using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Api.Models
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string Address { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
