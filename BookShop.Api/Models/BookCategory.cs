using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.Models
{
    public class BookCategory
    {
        
        public long BookId { get; set; }
        public Book Book { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }


    }
}
