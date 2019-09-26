using System;
using System.Collections.Generic;
using System.IO;
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
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "owner")]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [AllowAnonymous]
        // GET: api/books
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookRepository.GetBooksAsync();
            return Ok(books);
        }

        [AllowAnonymous]
        // GET: api/books/5
        [HttpGet("{id}", Name = "GetBook")]
        public async Task<IActionResult> GetBook(long id)
        {
            var aBook = await _bookRepository.GetBookAsync(id);
            return Ok(aBook);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "manager")]
        // POST: api/books
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromForm] AddBookRequestModel request)
        {
            request.Myuser = User;

            var book = await _bookRepository.CreateBookAsync(request);

            if (book != null)
            {
                return Ok(book);
            }

            return BadRequest();
        }

        // PUT: api/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(long id, [FromForm] UpdateBookRequestModel request)
        {

            var book = await _bookRepository.UpdateBookAsync(id, request);
            if (book != null)
            {
                return Ok(book);
            }

            return BadRequest();

        }

        // DELETE: api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            var aBook = await _bookRepository.FindAsync(id);

            if (aBook == null)
            {
                return NotFound();
            }

            await _bookRepository.DeleteBookAsync(id);
            return NoContent();
        }
    }
}