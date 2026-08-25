using System.Net;
using System.Net.Http.Json;
using Catalog.API.Features.Products.GetProducts;
using Catalog.API.IntegrationTests.Infrastructure.TestData;
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
        var response = await _fixture.Client.GetAsync("/api/v1/catalog/products?page_index=0&page_size=10");
        var result = await response.Content.ReadFromJsonAsync<GetProductsResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Products.Count.Should().Be(2);

        result.Products.Data.Should().Contain(x => x.Name == "MacBook Pro");
        result.Products.Data.Should().Contain(x => x.Name == "Surface Laptop");
    }

    [Fact]
    public async Task GetProducts_Should_Return_Empty_Result_When_No_Products_Exist()
    {
        // Act
        var response = await _fixture.Client.GetAsync("/api/v1/catalog/products?page_index=0&page_size=10");
        var result = await response.Content.ReadFromJsonAsync<GetProductsResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();

        result.Products.Should().NotBeNull();
        result.Products.Count.Should().Be(0);
        result.Products.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task GetProducts_Should_Respect_PageSize()
    {
        // Arrange
        await _fixture.SeedAsync(
            ProductData.MacBookPro(),
            ProductData.SurfaceLaptop(),
            ProductData.IPhone());

        // Act
        var response = await _fixture.Client.GetAsync("/api/v1/catalog/products?page_index=0&page_size=2");
        var result = await response.Content.ReadFromJsonAsync<GetProductsResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();

        result.Products.Count.Should().Be(3);
        result.Products.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetProducts_Should_Return_Correct_Products_When_Requesting_Second_Page()
    {
        // Arrange
        await _fixture.SeedAsync(
            ProductData.MacBookPro(),
            ProductData.SurfaceLaptop(),
            ProductData.IPhone());

        // Act
        var response = await _fixture.Client.GetAsync("/api/v1/catalog/products?page_index=1&page_size=2");
        var result = await response.Content.ReadFromJsonAsync<GetProductsResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();

        result.Products.Count.Should().Be(3);
        result.Products.Data.Should().HaveCount(1);
    }
}