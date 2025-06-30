using FluentValidation;
using Core.Application.Commands;
using PhoneNumbers;

namespace Core.Application.Validators
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of Birth is required.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.BankAccountNumber)
                .NotEmpty().WithMessage("Bank Account Number is required.")
                .Matches(@"^\d{9,18}$").WithMessage("Bank Account Number must be between 9 and 18 digits.");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Must(BeAValidMobileNumber).WithMessage("Invalid mobile phone number.");
        }

        private bool BeAValidMobileNumber(string phoneNumber)
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                PhoneNumber numberProto;

                if (phoneNumber.StartsWith("+"))
                {
                    // For international numbers, parse without region
                    numberProto = phoneUtil.Parse(phoneNumber, null);
                }
                else
                {
                    // Default region set to IR (can be changed if necessary)
                    numberProto = phoneUtil.Parse(phoneNumber, "IR");
                }

                // Check if the number is valid and of MOBILE type
                return phoneUtil.IsValidNumber(numberProto) &&
                       phoneUtil.GetNumberType(numberProto) == PhoneNumberType.MOBILE;
            }
            catch (NumberParseException)
            {
                // If parsing fails with region, try again without any region code
                try
                {
                    var numberProto = phoneUtil.Parse(phoneNumber, null);
                    return phoneUtil.IsValidNumber(numberProto) &&
                           phoneUtil.GetNumberType(numberProto) == PhoneNumberType.MOBILE;
                }
                catch (NumberParseException)
                {
                    // Log the exception if necessary
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                return false;
            }
        }
    }
}
