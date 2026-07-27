using Catalog.API.IntegrationTests.Infrastructure.Database;

namespace Catalog.API.IntegrationTests.Infrastructure.AutoFixture;

public class CatalogFixture : IAsyncLifetime
{
    private readonly TestDatabase _database = new();
    private CustomWebApplicationFactory? _factory;
    public HttpClient Client { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _database.StartAsync();
        _factory = new CustomWebApplicationFactory(_database.ConnectionString);
        Client = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        Client.Dispose();
        _factory?.Dispose();
        await _database.DisposeAsync();
    }
}