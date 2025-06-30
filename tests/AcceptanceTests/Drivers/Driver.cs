using Mc2.CrudTest.Presentation;

using Microsoft.AspNetCore.Mvc.Testing;

using Presentation.Api;

using System.Net.Http;

namespace Mc2.CrudTest.AcceptanceTests.Drivers
{
    public class Driver
    {
        public HttpClient Client { get; }

        public Driver()
        {
            var factory = new WebApplicationFactory<Program>();
            Client = factory.CreateClient();
        }

        // Additional helper methods can be added here
    }
}
