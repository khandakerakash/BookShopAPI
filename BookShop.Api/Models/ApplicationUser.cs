using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
