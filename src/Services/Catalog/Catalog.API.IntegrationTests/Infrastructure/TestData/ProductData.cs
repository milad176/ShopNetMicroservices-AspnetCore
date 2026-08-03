using Catalog.API.Models;

namespace Catalog.API.IntegrationTests.Infrastructure.TestData;

public static class ProductData

{
    public static Product MacBookPro() =>
        new ProductBuilder()
            .WithName("MacBook Pro")
            .WithCategory(["Laptop"])
            .WithDescription("Apple laptop")
            .WithImage("macbook.png")
            .WithPrice(2499)
            .Build();

    public static Product SurfaceLaptop() =>
        new ProductBuilder()
            .WithName("Surface Laptop")
            .WithCategory(["Laptop"])
            .WithDescription("Microsoft laptop")
            .WithImage("surface.png")
            .WithPrice(1999)
            .Build();
}