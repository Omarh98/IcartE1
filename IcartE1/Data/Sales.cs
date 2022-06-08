using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Data
{
    public class Sales
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int BranchId { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}
