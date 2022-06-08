using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Models
{
    public class BaseCategoryViewModel
    {
        public int Id { get; set; }
        [Required, RegularExpression(pattern: @"^[a-zA-Z\s]+$", ErrorMessage = "Category title can only contain letters.")]
        public string Title { get; set; }
       
        public IFormFile Image { get; set; }
    }
}
