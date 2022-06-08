using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Data
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ProductId { get; set; }
    }
}
