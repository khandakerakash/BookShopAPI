using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.Models
{
    public class Author
    {
        public long AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
