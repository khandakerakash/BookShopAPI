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

            if (request == null)
            {
                return BadRequest();
            }

            Book aBook = new Book()
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Quantity = request.Quantity,
                //CategoryId = request.CategoryId,
                IsApproved = false,
                AuthorId = request.AuthorId
            };

            if (request.Image != null && request.Image.Length > 0)
            {
                var fileName = Path.GetFileName(request.Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(fileSteam);
                }
                aBook.Image = fileName;
            }

            await _bookRepository.CreateBookAsync(aBook);
            return CreatedAtRoute(nameof(GetBook), new { id = aBook.BookId }, aBook);
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