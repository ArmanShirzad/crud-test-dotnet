using Presentation.Shared.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Client.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> GetCustomerByIdAsync(Guid id);
        Task<bool> AddCustomerAsync(CustomerDto customer);
        Task<bool> UpdateCustomerAsync(CustomerDto customer);
        Task<bool> DeleteCustomerAsync(Guid id);
    }
}
