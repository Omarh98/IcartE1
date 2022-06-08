using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Data
{
    public class Category
    {
        public int Id { get; set; }
        [Required, RegularExpression(pattern: @"^[a-zA-Z\s]+$", ErrorMessage = "Category title can only contain letters.")]
        public string Title { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
