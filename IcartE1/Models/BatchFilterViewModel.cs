using IcartE1.Data;
using System.Collections.Generic;

namespace IcartE1.Models
{
    public class BatchFilterViewModel
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int BranchId { get; set; }
        public ICollection<Batch> Batches { get; set; }
    }
}
