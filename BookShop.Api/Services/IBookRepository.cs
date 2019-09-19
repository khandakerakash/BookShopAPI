using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookShop.Api.Models;

namespace BookShop.Api.Services
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookAsync(long id);
        Task CreateBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(long id);

        Task<Book> FindAsync(long id);
        Task<Author> GetAuthorByIdAsync(long id);
        Task<Category> GetCategoryByIdAsync(long id);

        Task<bool> AuthorExists(long id, CancellationToken token);
        Task<bool> CategoryExists(long id, CancellationToken token);
    }
}
