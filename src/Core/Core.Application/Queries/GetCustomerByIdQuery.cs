using MediatR;
using Core.Domain.Entities;
using System;

namespace Core.Application.Queries
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public Guid Id { get; set; }
    }
}
