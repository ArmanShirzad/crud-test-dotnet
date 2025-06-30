using MediatR;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Queries
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly ICustomerRepository _repository;

        public GetCustomerByIdQueryHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetCustomerByIdAsync(request.Id);
        }
    }
}
