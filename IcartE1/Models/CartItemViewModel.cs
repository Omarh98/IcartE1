using IcartE1.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Models
{
    public class CartItemViewModel
    {
        public Product Product { get; set; }
        public int BatchId { get; set; }

        public int Quantity { get; set; }
    }
}
