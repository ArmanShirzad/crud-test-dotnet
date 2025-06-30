using MediatR;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;
using FluentValidation;

namespace Core.Application.Commands
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _repository;
        private readonly IValidator<CreateCustomerCommand> _validator;


        public CreateCustomerCommandHandler(ICustomerRepository repository, IValidator<CreateCustomerCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            // Check for uniqueness
            if (!await _repository.IsEmailUniqueAsync(request.Email))
                throw new Exception("Email must be unique.");

            if (!await _repository.IsCustomerUniqueAsync(request.FirstName, request.LastName, request.DateOfBirth))
                throw new Exception("Customer must be unique by First Name, Last Name, and Date of Birth.");

            // Parse phone number to ulong
            if (!ulong.TryParse(request.PhoneNumber, out ulong phoneNumber))
                throw new Exception("Invalid phone number format.");

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = phoneNumber,
                Email = request.Email,
                BankAccountNumber = request.BankAccountNumber
            };

            await _repository.AddCustomerAsync(customer);

            return customer.Id;
        }
    }
}
