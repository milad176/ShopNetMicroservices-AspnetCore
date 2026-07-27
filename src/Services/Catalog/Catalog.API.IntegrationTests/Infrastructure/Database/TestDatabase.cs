using Testcontainers.PostgreSql;

namespace Catalog.API.IntegrationTests.Infrastructure.Database;

public sealed class TestDatabase : IAsyncDisposable
{
    private readonly PostgreSqlContainer _container =
        new PostgreSqlBuilder()
            .WithDatabase("CatalogDb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public string ConnectionString => _container.GetConnectionString();

    public async Task StartAsync()
    {
        await _container.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _container.StopAsync();
    }
}