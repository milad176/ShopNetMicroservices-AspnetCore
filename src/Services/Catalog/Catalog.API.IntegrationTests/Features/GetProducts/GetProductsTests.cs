using System.Net;
using System.Net.Http.Json;
using Catalog.API.Features.Products.GetProducts;
using Catalog.API.IntegrationTests.Infrastructure.AutoFixture;
using Catalog.API.IntegrationTests.Infrastructure.TestData;
using Catalog.API.Models;
using FluentAssertions;

namespace Catalog.API.IntegrationTests.Features.GetProducts;

[Collection("Catalog")]
public class GetProductsTests
{
    private readonly CatalogFixture _fixture;

    public GetProductsTests(CatalogFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact]
    public async Task GetProducts_Should_Return_All_Products()
    {
        // Arrange
        await _fixture.SeedAsync(
            ProductData.MacBookPro(),
            ProductData.SurfaceLaptop());

        // Act 
        var response = await _fixture.Client.GetAsync("/api/v1/catalog/products?pageIndex=0&pageSize=10");
        var result = await response.Content.ReadFromJsonAsync<GetProductsResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Products.Should().NotBeNull();
        result.Products.Count.Should().Be(2);

        result.Products.Data.Should().Contain(x => x.Name == "MacBook Pro");
        result.Products.Data.Should().Contain(x => x.Name == "Surface Laptop");
    }
}