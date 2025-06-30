using Core.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(Guid id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Guid id);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsCustomerUniqueAsync(string firstName, string lastName, DateTime dateOfBirth);
    }
}
