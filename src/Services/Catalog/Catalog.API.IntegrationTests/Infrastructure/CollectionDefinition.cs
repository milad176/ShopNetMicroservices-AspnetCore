using Catalog.API.IntegrationTests.Infrastructure.AutoFixture;

namespace Catalog.API.IntegrationTests.Infrastructure;

[CollectionDefinition("Catalog")]
public class CatalogCollection : ICollectionFixture<CatalogFixture>
{
}