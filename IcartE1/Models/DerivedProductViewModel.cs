using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Models
{
    public class DerivedProductViewModel : BaseProductViewModel
    {
        [Required]
        public new IFormFileCollection Images { get; set; }
    }
}
