using Catalog.API.IntegrationTests.Infrastructure.AutoFixture;

namespace Catalog.API.IntegrationTests.Infrastructure.Collections;

[CollectionDefinition("Catalog")]
public class CatalogCollection : ICollectionFixture<CatalogFixture>
{
}