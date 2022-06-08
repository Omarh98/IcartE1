using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Models
{
    public class DerivedCategoryViewModel : BaseCategoryViewModel
    {
    
        [Required]
        public new IFormFile Image { get; set; }
    }
}
