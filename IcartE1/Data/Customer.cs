using IcartE1.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE.Data
{
    public class Customer
    {
        public string Id { get; set; }

        [Required, Display(Name = "First Name")]
        [RegularExpression(pattern: @"^[a-zA-Z]+$", ErrorMessage = "First Name can only contain letters.")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        [RegularExpression(pattern: @"^[a-zA-Z]+$", ErrorMessage = "Last Name can only contain letters.")]
        public string LastName { get; set; }
        [Required, DataType(dataType: DataType.Date), Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Range(minimum: 0, maximum: Double.MaxValue, ErrorMessage = "Reward points must be greater than 0")]
        public int RewardPoints { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }

        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; }

        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
