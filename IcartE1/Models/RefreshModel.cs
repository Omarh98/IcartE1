using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Models
{
    public class RefreshModel
    {
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public string AccessToken { get; set; }
    }
}
