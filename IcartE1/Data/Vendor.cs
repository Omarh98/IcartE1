using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Data
{
    public class Vendor
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required, RegularExpression(pattern: "^[0]{1}[1]{1}[0-9]{9}$", ErrorMessage = "Phone number"), Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
