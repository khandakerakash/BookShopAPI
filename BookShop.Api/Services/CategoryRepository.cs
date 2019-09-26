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
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(long id);
        Task<Category> CreateCategoryAsync(AddCategoryRequestModel requestModel);
        Task<Category> UpdateCategoryAsync(long id, UpdateCategoryRequestModel request);
        Task DeleteCategoryAsync(long id);

        Task<Category> FindAsync(long id);

        Task<bool> CheckNameAlreadyExits(string name, CancellationToken token);
        //Task<bool> CategoryExists(long id, string title);
    }
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

        public async Task<Category> CreateCategoryAsync(AddCategoryRequestModel requestModel)
        {
            if (requestModel.Name != null)
            {
                requestModel.Name = requestModel.Name;
            }

            if (requestModel.Description != null)
            {
                requestModel.Description = requestModel.Description;
            }

            Category aCategory = new Category()
            {
                Name = requestModel.Name,
                Description = requestModel.Description,
            };

            var applicationUserId = requestModel.Myuser.Claims.Where(c => c.Type == "sub")
                .Select(c => c.Value).SingleOrDefault();

            aCategory.ApplicationUserId = Convert.ToInt64(applicationUserId);

            await _context.AddAsync(aCategory);

            if (await _context.SaveChangesAsync() > 0)
            {
                return aCategory;
            }

            return null;
        }

        public async Task<Category> UpdateCategoryAsync(long id, UpdateCategoryRequestModel request)
        {
            var category = await FindAsync(id);
            if (category == null)
            {
                return null;
            }

            if (request.Name != null)
            {
                category.Name = request.Name;
            }

            if (request.Description != null)
            {
                category.Description = request.Description;
            }


            _context.Categories.Update(category);
            if (await _context.SaveChangesAsync() > 0)
            {
                return category;
            }

            return null;
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


        public async Task<bool> CheckNameAlreadyExits(string name, CancellationToken token)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == name);
            return category == null;
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
