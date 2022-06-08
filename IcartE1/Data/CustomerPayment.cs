using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Data
{
    public class CustomerPayment
    {
        public int Id { get; set; }

        [Required, Display(Name = "Holder Name")]
        [RegularExpression(pattern: @"^[a-zA-Z\s]+$", ErrorMessage = "Holder name can only contain letters.")]
        public string HolderName { get; set; }

        [Required, Display(Name = "Card Number")]
        [CreditCard]
        public string CardNumber { get; set; }
        [Required, DataType(dataType: DataType.Date), Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        public string CustomerId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
