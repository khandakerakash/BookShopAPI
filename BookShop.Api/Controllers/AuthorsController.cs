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
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [AllowAnonymous]
        // GET: api/authors
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorRepository.GetAuthorsAsync();
            return Ok(authors);
        }

        [AllowAnonymous]
        // GET: api/authors/5
        [HttpGet("{id}", Name = "GetAuthor")]
        public async Task<IActionResult> GetAuthor(long id)
        {
            var author = await _authorRepository.GetAuthorAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        // POST: api/authors
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromForm] AuthorRequestModel request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            Author author = new Author()
            {
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            await _authorRepository.CreateAuthorAsync(author);

            return CreatedAtRoute(nameof(GetAuthor), new {id = author.AuthorId}, author);
        }

        // PUT: api/authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(long id, [FromForm] AuthorRequestModel request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var aAuthor = await _authorRepository.FindAsync(id);

            if (aAuthor == null)
            {
                return NotFound();
            }

            Author author = new Author()
            {
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            await _authorRepository.UpdateAuthorAsync(author);

            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(long id)
        {
            var aAuthor = await _authorRepository.FindAsync(id);

            if (aAuthor == null)
            {
                return NotFound();
            }

            await _authorRepository.DeleteAuthorAsync(id);
            return NoContent();
        }
    }
}