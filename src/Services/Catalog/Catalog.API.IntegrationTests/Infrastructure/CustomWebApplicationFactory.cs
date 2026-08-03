using Catalog.API.Models;
using Marten;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalog.API.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public CustomWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IDocumentStore));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddMarten(opts =>
                {
                    opts.Connection(_connectionString);
                    opts.Schema.For<Product>().UseNumericRevisions(true);
                })
                .UseLightweightSessions();
        });
    }
}