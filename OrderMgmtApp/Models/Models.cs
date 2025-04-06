using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderMgmtApp.Models
{
    public class TableOrder
    {
        public int ID { get; set; }
        public string GuestName { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public Product BaseProduct { get; set; }
        public int Quantity { get; set; }
        public List<ProductModifier> Modifiers { get; set; }
    }

    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class ProductModifier
    {
        public string Name { get; set; }
        public double ExtraPrice { get; set; }
    }
}
