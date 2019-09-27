using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookShop.Api.Contexts;
using BookShop.Api.Models;
using BookShop.Api.RequestResponse.Request;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Api.Services
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAuthorsAsync();
        Task<Author> GetAuthorAsync(long id);
        Task<Author> CreateAuthorAsync(AddAuthorRequestModel request);
        Task<Author> UpdateAuthorAsync(long id, UpdateAuthorRequestModel request);
        Task DeleteAuthorAsync(long id);

        Task<Author> FindAsync(long id);
    }

    public class AuthorRepository : IAuthorRepository, IDisposable
    {
        private ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await _context.Authors.Include(x => x.Books).ToListAsync();
        }

        public async Task<Author> GetAuthorAsync(long id)
        {
            return await _context.Authors.Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.AuthorId == id);
        }

        public async Task<Author> CreateAuthorAsync(AddAuthorRequestModel request)
        {
            Author author = new Author()
            {
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var applicationUserId = request.Myuser.Claims.Where(c => c.Type == "sub")
                .Select(c => c.Value).SingleOrDefault();

            author.ApplicationUserId = Convert.ToInt64(applicationUserId);

            await _context.AddAsync(author);

            if (await _context.SaveChangesAsync() > 0)
            {
                return author;
            }

            return null;
        }

        public async Task<Author> UpdateAuthorAsync(long id, UpdateAuthorRequestModel request)
        {
            var author = await FindAsync(id);

            if (author == null)
            {
                return null;
            }

            if (request.FirstName != null)
            {
                author.FirstName = request.FirstName;
            }

            if (request.LastName != null)
            {
                author.LastName = request.LastName;
            }

            _context.Authors.Update(author);

            if (await _context.SaveChangesAsync() > 0)
            {
                return author;
            }

            return null;
        }

        public async Task DeleteAuthorAsync(long id)
        {
            var authorToRemove = await _context.Authors.FirstOrDefaultAsync(x => x.AuthorId == id);

            if (authorToRemove != null)
            {
                _context.Authors.Remove(authorToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Author> FindAsync(long id)
        {
            return await _context.Authors.Where(x => x.AuthorId.Equals(id)).FirstOrDefaultAsync();
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
