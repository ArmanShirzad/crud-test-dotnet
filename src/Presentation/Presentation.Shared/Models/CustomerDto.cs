using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Shared.Models
{
    public class CustomerDto
    {
        public Guid Id { get; set; } 

        [Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Birth date is required.we may give u a gift!")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Phone number must start with an optional '+' followed by digits only.")]
        public string? PhoneNumber { get; set; } // Keep as string

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required]
        public string? BankAccountNumber { get; set; }
    }
}
