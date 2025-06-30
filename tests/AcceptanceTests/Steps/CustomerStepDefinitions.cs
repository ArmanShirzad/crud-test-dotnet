using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

using Infrastructure.Persistence.Contexts;

using Mc2.CrudTest.Presentation;

using Microsoft.AspNetCore.Mvc.Testing;

using Presentation.Api ;
using Presentation.Shared.Models;

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using TechTalk.SpecFlow;

namespace Mc2.CrudTest.AcceptanceTests.Steps
{
    [Binding]
    public class CustomerStepDefinitions
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private CustomerDto? _newCustomer;
        private HttpResponseMessage? _postResponse;
        private CustomerDto? _retrievedCustomer;

        public CustomerStepDefinitions()
        {
            _factory = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<MainDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<MainDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
            });
        });

    
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                HandleCookies = false,
                AllowAutoRedirect = false
            });

            _client.BaseAddress = new Uri("https://localhost:5091/");
        }

        [Given(@"I have navigated to the Add Customer page")]
        public void GivenIHaveNavigatedToTheAddCustomerPage()
        {
            // In API testing, navigation is abstract. This step can be a placeholder.
            // Alternatively, use browser automation tools like Selenium for full UI testing.
        }

        [When(@"I fill in the customer details with valid information")]
        public void WhenIFillInTheCustomerDetailsWithValidInformation()
        {
            _newCustomer = new CustomerDto
            {
                FirstName = "Acceptance",
                LastName = "Test",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "+989333627591",
                Email = "acceptance.test@example.com",
                BankAccountNumber = "123456789"
            };
        }

        [When(@"I submit the form")]
        public async Task WhenISubmitTheForm()
        {
            _postResponse = await _client.PostAsJsonAsync("api/customers", _newCustomer);
        }

        [Then(@"the customer should be successfully added")]
        public void ThenTheCustomerShouldBeSuccessfullyAdded()
        {
            _postResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            _postResponse.Headers.Location.Should().NotBeNull();
        }

        [Then(@"the customer should appear in the Customer List")]
        public async Task ThenTheCustomerShouldAppearInTheCustomerList()
        {
            var location = _postResponse.Headers.Location.ToString();
            _retrievedCustomer = await _client.GetFromJsonAsync<CustomerDto>(location);

            _retrievedCustomer.Should().NotBeNull();
            _retrievedCustomer.FirstName.Should().Be(_newCustomer.FirstName);
            _retrievedCustomer.LastName.Should().Be(_newCustomer.LastName);
            _retrievedCustomer.Email.Should().Be(_newCustomer.Email);
        }
    }
}
