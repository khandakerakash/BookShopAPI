using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Contexts;
using BookShop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Api.Services
{
    public class CategoryRepository : ICategoryRepository, IDisposable
    {
        private ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.Include(x => x.BookCategories).ToListAsync();
        }

        public async Task<Category> GetCategoryAsync(long id)
        {
            return await _context.Categories.Include(x => x.BookCategories)
                .FirstOrDefaultAsync(x => x.CategoryId == id);
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var categoryToUpdate =
                await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == category.CategoryId);

            if (categoryToUpdate != null)
            {
                categoryToUpdate.Name = category.Name;
                categoryToUpdate.Description = category.Description;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCategoryAsync(long id)
        {
            var categoryToDelete = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);

            if (categoryToDelete != null)
            {
                _context.Categories.Remove(categoryToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Category> FindAsync(long id)
        {
            return await _context.Categories.Where(x => x.CategoryId.Equals(id)).FirstOrDefaultAsync();
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
