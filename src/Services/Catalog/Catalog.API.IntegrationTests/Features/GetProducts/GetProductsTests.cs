using Catalog.API.IntegrationTests.Infrastructure.AutoFixture;

namespace Catalog.API.IntegrationTests.Features.GetProducts;

[Collection("Catalog")]
public class GetProductsTests
{
    private readonly CatalogFixture _fixture;

    public GetProductsTests(CatalogFixture fixture)
    {
        _fixture = fixture;
    }
}