using System.ComponentModel.DataAnnotations;

namespace IcartE1.Models
{
    public class AddCartItemViewModel
    {
        [Required,Range(minimum:1,maximum:double.MaxValue)]
        public int ProductId { get; set; }
        [Required,Range(minimum: 0, maximum: double.MaxValue)]
        public int BatchId { get; set; }
        [Required,Range(minimum: 1, maximum: double.MaxValue)]
        public int Quantity { get; set; }
    }
}
