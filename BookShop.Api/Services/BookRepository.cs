using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookShop.Api.Contexts;
using BookShop.Api.Models;
using BookShop.Api.RequestResponse.Request;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Api.Services
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookAsync(long id);
        Task<Book> CreateBookAsync(AddBookRequestModel request);
        Task<Book> UpdateBookAsync(long id, UpdateBookRequestModel request);
        Task DeleteBookAsync(long id);

        Task<Book> FindAsync(long id);
        Task<Author> GetAuthorByIdAsync(long id);
        Task<Category> GetCategoryByIdAsync(long id);

        Task<bool> AuthorExists(long id, CancellationToken token);
        Task<bool> CategoryExists(long id, CancellationToken token);
    }

    public class BookRepository : IBookRepository, IDisposable
    {
        private ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            //return await _context.Books.Include(x => x.Author).Include(x => x.BookCategories).ToListAsync();
            //return await _context.Books.Include(x => x.BookCategories)
            //.Include(x => x.ApplicationUser.UserName).ToListAsync();

            return await _context.Books.Include(x => x.BookCategories).ThenInclude(x=>x.Category).ToListAsync();
        }

        public async Task<Book> GetBookAsync(long id)
        {
            return await _context.Books.Include(x => x.BookCategories)
                .ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.BookId == id);
        }

        public async Task<Book> CreateBookAsync(AddBookRequestModel request)
        {
            var  categories = await _context.Categories
                .Where(x => request.CategoryId.Contains(x.CategoryId)).ToListAsync();

            var bookCategory = new List<BookCategory>();
            foreach (var category in categories)
            {
                bookCategory.Add(new BookCategory()
                {
                    Category = category
                });
            }
            Book book = new Book()
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Quantity = request.Quantity,
                IsApproved = false,
                AuthorId = request.AuthorId,
                BookCategories = bookCategory
            };

            if (request.Image != null && request.Image.Length > 0)
            {
                var fileName = Path.GetFileName(request.Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(fileSteam);
                }
                book.Image = fileName;
            }

            var applicationUserId = request.Myuser.Claims.Where(c => c.Type == "sub")
                .Select(c => c.Value).SingleOrDefault();

            book.ApplicationUserId = Convert.ToInt64(applicationUserId);

            await _context.AddAsync(book);

            if (await _context.SaveChangesAsync() > 0)
            {
                return book;
            }

            return null;
        }

        public async Task<Book> UpdateBookAsync(long id, UpdateBookRequestModel request)
        {
            var categories = await _context.Categories
                .Where(x => request.CategoryId.Contains(x.CategoryId)).ToListAsync();

            var bookCategory = new List<BookCategory>();

            foreach (var category in categories)
            {
                bookCategory.Add(new BookCategory()
                {
                    Category = category
                });
            }

            var book = await FindAsync(id);


            if (book == null)
            {
                return null;
            }

            if (bookCategory != null)
            {
                book.BookCategories = bookCategory;
            }


            if (request.Title != null)
            {
                book.Title = request.Title;
            }

            if (request.Description != null)
            {
                book.Description = request.Description;
            }


            if (request.Price != 0)
            {
                book.Price = request.Price;
            }

            if (request.AuthorId != 0)
            {
                book.AuthorId = request.AuthorId;
            }

            _context.Books.Update(book);
            if (await _context.SaveChangesAsync() > 0)
            {
                return book;
            }

            return null;
        }

        public async Task DeleteBookAsync(long id)
        {
            var bookToRemove = await _context.Books.FirstOrDefaultAsync(x => x.BookId == id);

            if (bookToRemove != null)
            {
                _context.Books.Remove(bookToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Book> FindAsync(long id)
        {
            return await _context.Books.FirstOrDefaultAsync(x => x.BookId.Equals(id));
        }

        public async Task<Author> GetAuthorByIdAsync(long id)
        {
            return await _context.Authors.FirstOrDefaultAsync(x => x.AuthorId == id);
        }

        public async Task<Category> GetCategoryByIdAsync(long id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
        }

        public async Task<bool> AuthorExists(long id, CancellationToken token)
        {
            var info = await GetAuthorByIdAsync(id);

            if (info == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CategoryExists(long id, CancellationToken token)
        {
            var info = await GetCategoryByIdAsync(id);

            if (info == null)
            {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}
