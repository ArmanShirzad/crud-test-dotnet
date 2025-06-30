using Presentation.Shared.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Presentation.Client.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(HttpClient httpClient, ILogger<CustomerService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            _logger.LogInformation("GetAllCustomersAsync called");
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<CustomerDto>>("api/customers");
                _logger.LogInformation("GetAllCustomersAsync completed successfully");
                return result ?? new List<CustomerDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get customers: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(Guid id)
        {
            _logger.LogInformation("GetCustomerByIdAsync called");

            var customer = await _httpClient.GetFromJsonAsync<CustomerDto>($"api/customers/{id}");
            if (customer == null)
            {
                throw new Exception("Customer not found.");
            }
            return customer;
        }

        public async Task<bool> AddCustomerAsync(CustomerDto customer)
        {
            _logger.LogInformation("AddCustomerAsync called");

            var response = await _httpClient.PostAsJsonAsync("api/customers", customer);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCustomerAsync(CustomerDto customer)
        {
            _logger.LogInformation("inside update method");

            var response = await _httpClient.PutAsJsonAsync($"api/customers/{customer.Id}", customer);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/customers/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
