using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.Models
{
    public class Order
    {
        public long OrderId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipZipCode { get; set; }
        public string ShipCountry { get; set; }

        public long ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        
    }
}
