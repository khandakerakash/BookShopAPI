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

        public async Task CreateAuthorAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            var authorToUpdate = await _context.Authors.FirstOrDefaultAsync(x => x.AuthorId == author.AuthorId);

            if (authorToUpdate !=null)
            {
                authorToUpdate.FirstName = author.FirstName;
                authorToUpdate.LastName = author.LastName;
                await _context.SaveChangesAsync();
            }
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
