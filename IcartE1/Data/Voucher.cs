using System;
using System.ComponentModel.DataAnnotations;

namespace IcartE1.Data
{
    public class Voucher
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float Value { get; set; }
        [Required]
        public float MinOrder { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public string CustomerId { get; set; }
    }
}
