using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Data
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required, Range(minimum: 0.49, maximum: double.MaxValue)]
        public float Price { get; set; }
        [Required,Display(Name ="Reorder Quantity")]
        public int ReorderQuantity { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public virtual ICollection<Batch> Batches { get; set; }

        public virtual ICollection<ListItem> ListItems { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        [Display(Name ="Images")]
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
    }
}
