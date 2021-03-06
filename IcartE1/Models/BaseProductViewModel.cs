using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Models
{
    public class BaseProductViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required, Range(minimum: 0.49, maximum: double.MaxValue)]
        public float Price { get; set; }
        [Required,Display(Name ="Reorder Quantity")]
        public int ReorderQuantity { get; set; }
        [Required,Display(Name ="Category")]
        public int CategoryId { get; set; }
        [Required,Display(Name ="Vendor")]
        public int VendorId { get; set; }

        public IFormFileCollection Images { get; set; }
    }
}
