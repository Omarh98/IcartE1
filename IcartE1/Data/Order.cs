using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Data
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public DateTime DateOrdered { get; set; }
        [Required]
        public bool IsCash { get; set; }
        [Required]
        public float Discount { get; set; }
        [Required]
        public float SubTotal { get; set; }
        [Required]
        public float Total { get; set; }
        [Required]
        public string CustomerId { get; set; }
        public int? CustomerPaymentId { get; set; }
        [Required]
        public int BranchId { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
