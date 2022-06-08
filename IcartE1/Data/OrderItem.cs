using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Data
{
    public class OrderItem
    {
        [Column(Order = 0), ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        [Column(Order = 1), ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required, Range(minimum: 1, maximum: double.MaxValue)]
        public int Quantity { get; set; }
    }
}
