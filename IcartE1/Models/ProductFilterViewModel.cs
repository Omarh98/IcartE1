using IcartE1.Data;
using System.Collections.Generic;

namespace IcartE1.Models
{
    public class ProductFilterViewModel
    {
        public int CategoryId { get; set; }
        public int VendorId { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
