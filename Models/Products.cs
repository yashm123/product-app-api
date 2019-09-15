using System;
using System.Collections.Generic;

namespace ProductApi.Models
{
    public partial class Products
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
