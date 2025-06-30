using MediatR;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Core.Application.Commands
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _repository;

        public UpdateCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetCustomerByIdAsync(request.Id);
            if (customer == null)
                throw new Exception("Customer not found.");

            if (customer.Email != request.Email && !await _repository.IsEmailUniqueAsync(request.Email))
                throw new Exception("Email must be unique.");

            if ((customer.FirstName != request.FirstName || customer.LastName != request.LastName || customer.DateOfBirth != request.DateOfBirth) &&
                !await _repository.IsCustomerUniqueAsync(request.FirstName, request.LastName, request.DateOfBirth))
                throw new Exception("Customer must be unique by First Name, Last Name, and Date of Birth.");

            if (!ulong.TryParse(request.PhoneNumber, out ulong phoneNumber))
                throw new Exception("Invalid phone number format.");

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.DateOfBirth = request.DateOfBirth;
            customer.PhoneNumber = phoneNumber;
            customer.Email = request.Email;
            customer.BankAccountNumber = request.BankAccountNumber;

            await _repository.UpdateCustomerAsync(customer);
            return true;
        }
    }
}
