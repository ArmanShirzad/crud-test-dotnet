using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public ulong PhoneNumber { get; set; } // Using ulong to minimize storage space as instructed by the requirment.
        public string Email { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;

        public void SetPhoneNumber(string phoneNumber)
        {
            if (ulong.TryParse(phoneNumber, out var parsedNumber))
            {
                PhoneNumber = parsedNumber;
            }
            else
            {
                throw new ArgumentException("Invalid phone number format");
            }
        }

        public string GetPhoneNumber() => PhoneNumber.ToString();
    }


}
