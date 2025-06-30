using MediatR;

using System;

namespace Core.Application.Commands
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteCustomerCommand() { }
        public DeleteCustomerCommand(Guid id)
        {
            Id = id;
        }
    }
}
