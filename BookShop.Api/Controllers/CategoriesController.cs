using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Models;
using BookShop.Api.RequestResponse.Request;
using BookShop.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "owner")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [AllowAnonymous]
        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return Ok(categories);
        }

        [AllowAnonymous]
        // GET: api/categories/5
        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<IActionResult> GetCategory(long id)
        {
            var category = await _categoryRepository.GetCategoryAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] AddCategoryRequestModel request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            Category aCategory = new Category()
            {
                Name = request.Name,
                Description = request.Description
            };

            await _categoryRepository.CreateCategoryAsync(aCategory);

            return CreatedAtRoute(nameof(GetCategory), new {id = aCategory.CategoryId}, aCategory);
        }

        // PUT: api/categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(long id, [FromForm] UpdateCategoryRequestModel request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var aCategory = await _categoryRepository.FindAsync(id);

            if (aCategory == null)
            {
                return NotFound();
            }

            Category category = new Category()
            {
                Name = request.Name,
                Description = request.Description
            };

            await _categoryRepository.UpdateCategoryAsync(category);
            return NoContent();
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            var aCategory = await _categoryRepository.FindAsync(id);

            if (aCategory == null)
            {
                return NotFound();
            }

            await _categoryRepository.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}