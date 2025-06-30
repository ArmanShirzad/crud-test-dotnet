using MediatR;
using Core.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Core.Application.Commands
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepository _repository;

        public DeleteCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetCustomerByIdAsync(request.Id);
            if (customer == null)
                throw new Exception("Customer not found.");

            await _repository.DeleteCustomerAsync(request.Id);
            return true;
        }
    }
}
