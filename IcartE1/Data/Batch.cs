using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Data
{
    public class Batch
    {
        public int Id { get; set; }
        [Required, Range(minimum: 0, maximum: Double.MaxValue)]
        public int Quantity { get; set; }
        [Required, DataType(dataType: DataType.Date), Display(Name = "Arrival Date")]
        public DateTime ArrivalDate { get; set; }

        [Required, DataType(dataType: DataType.Date), Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        public int? BranchId { get; set; }
        public Branch Branch { get; set; }
        public int? WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public string QrImageUrl { get; set; }
    }
}
