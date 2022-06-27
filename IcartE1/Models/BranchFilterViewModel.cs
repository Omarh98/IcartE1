using System.ComponentModel.DataAnnotations;

namespace IcartE1.Models
{
    public class BranchFilterViewModel
    {
        [Required]
        public bool IsOnline { get; set; }
        [Required]
        public double latitude { get; set; }
        [Required]
        public double longitude { get; set; }
    }
}
