using System;
using System.Collections.Generic;

namespace ProductApi.Models
{
    public partial class Cart
    {
        public string EmailId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public int SNo { get; set; }
    }
}
