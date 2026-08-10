using Marten;

namespace Catalog.API.IntegrationTests.Infrastructure.AutoFixture;

public class CatalogFixture : IAsyncLifetime
{
    private readonly TestDatabase _database = new();
    private CustomWebApplicationFactory _factory = null!;
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
        _factory.Dispose();

        await _database.DisposeAsync();
    }

    public IServiceProvider Services => _factory.Services;

    public IDocumentStore DocumentStore =>
        Services.GetRequiredService<IDocumentStore>();

    public async Task SeedAsync(params Product[] products)
    {
        await using var session = DocumentStore.LightweightSession();

        foreach (var product in products)
        {
            session.Store(product);
        }

        await session.SaveChangesAsync();
    }
}