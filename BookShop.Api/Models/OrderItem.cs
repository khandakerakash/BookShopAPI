using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.Models
{
    public class OrderItem
    {
        public long OrderItemId { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemQuantity { get; set; }

        public long OrderId { get; set; }
        public Order Order { get; set; }

        public long BookId { get; set; }
        public Book Book { get; set; }
    }
}
