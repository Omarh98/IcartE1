using System.ComponentModel.DataAnnotations;

namespace IcartE1.Models
{
    public class UpdateAddressViewModel
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
    }
}
