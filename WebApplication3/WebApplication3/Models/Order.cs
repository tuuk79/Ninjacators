using System.Collections.Generic;

namespace WebApplication3.Models
{
    public class Order
    {
        public string id { get; set; }
        public Contact contact { get; set; }
        public List<OrderItem> order_items { get; set; }
    }
}