using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Data
{
    public class ShoppingList
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }

        public string CustomerId { get; set; }

        public virtual ICollection<ListItem> ListItems { get; set; }
    }
}
