using System.ComponentModel.DataAnnotations;

namespace IcartE1.Models
{
    public class AddOrderViewModel
    {
        public bool IsCash { get; set; }
        [Required,Range(0,double.MaxValue)]
        public int PaymentId { get; set; }
        [Required,Range(1,double.MaxValue)]
        public int BranchId { get; set; }
        public bool IsOnline { get; set; }
        public int? VoucherId { get; set; }

    }
}
