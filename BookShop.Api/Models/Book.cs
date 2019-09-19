using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.Models
{
    public class Book
    {
        public long BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsApproved { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public long AuthorId { get; set; }
        public Author Author { get; set; }
        //public long CategoryId { get; set; }
        public List<BookCategory> BookCategories { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
