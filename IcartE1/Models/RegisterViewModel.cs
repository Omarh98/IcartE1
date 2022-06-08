using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Models
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Display(Name = "Phone Number")]
        [RegularExpression(pattern: @"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Phone Number is invalid.")]
        public string PhoneNumber { get; set; }

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

        [Required]
        public string Address { get; set; }

        [Required, Compare(otherProperty: nameof(ConfirmPassword), ErrorMessage = "Password and Confirm Password do not match.")]
        public string Password { get; set; }
        [Required, Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
