using MediatR;
using Core.Domain.Entities;
using System.Collections.Generic;

namespace Core.Application.Queries
{
    public class GetAllCustomersQuery : IRequest<IEnumerable<Customer>>
    {
    }
}
