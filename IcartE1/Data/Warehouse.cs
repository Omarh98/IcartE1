using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Data
{
    public class Warehouse
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        public virtual ICollection<Batch> Batches { get; set; }

    }
}
