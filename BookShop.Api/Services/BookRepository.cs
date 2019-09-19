using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookShop.Api.Contexts;
using BookShop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Api.Services
{
    public class BookRepository : IBookRepository, IDisposable
    {
        private ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.Include(x => x.Author)
                .Include(x => x.BookCategories)
                .ToListAsync();
        }

        public async Task<Book> GetBookAsync(long id)
        {
            return await _context.Books.Include(x => x.Author)
                .Include(x => x.BookCategories)
                .FirstOrDefaultAsync(x => x.BookId == id);
        }

        public async Task CreateBookAsync(Book book)
        {
            await _context.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            var bookToUpdate = await _context.Books.FirstOrDefaultAsync(x => x.BookId == book.BookId);

            if (bookToUpdate != null)
            {
                bookToUpdate.Title = book.Title;
                bookToUpdate.Description = book.Description;
                bookToUpdate.Image = book.Image;
                bookToUpdate.Price = book.Price;
                bookToUpdate.Quantity = book.Quantity;
                await _context.SaveChangesAsync();
            }
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
