using Catalog.API.Models;

namespace Catalog.API.IntegrationTests.Infrastructure.TestData;

public class ProductBuilder
{
    private readonly Product _product = new()
    {
        Id = Guid.NewGuid(),
        Name = "Test Product",
        Description = "Test Description",
        ImageFile = "image.png",
        Price = 100,
        Category = ["Test Category"]
    };

    public ProductBuilder WithName(string name)
    {
        _product.Name = name;
        return this;
    }

    public ProductBuilder WithDescription(string description)
    {
        _product.Description = description;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _product.Price = price;
        return this;
    }

    public ProductBuilder WithCategory(List<string> categories)
    {
        _product.Category = categories;
        return this;
    }

    public ProductBuilder WithImage(string imageFile)
    {
        _product.ImageFile = imageFile;
        return this;
    }

    public Product Build() => _product;
}