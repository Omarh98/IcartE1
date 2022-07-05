using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Models
{
    public class ProductStocksViewModel
    {
        [Key]
        public int Key { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ReorderQuantity { get; set; }
        [Display(Name ="Warehouse")]
        public string WarehouseTitle { get; set; }
        [Display(Name = "Branch")]
        public string BranchTitle { get; set; }
        public int BranchId { get; set; }
        public int WarehouseId { get; set; }
    }
}
