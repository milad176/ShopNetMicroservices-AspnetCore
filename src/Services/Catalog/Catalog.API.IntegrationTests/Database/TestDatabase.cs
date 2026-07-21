using Testcontainers.PostgreSql;

namespace Catalog.API.IntegrationTests.Database;

public sealed class TestDatabase
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

    public async Task StopAsync()
    {
        await _container.StopAsync();
    }
}