using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.Models
{
    public class Category
    {
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public long ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


        public ICollection<BookCategory> BookCategories { get; set; }

    }
}
