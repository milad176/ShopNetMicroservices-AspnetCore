namespace Catalog.API.Features.Products.CreateProduct;

public record CreateProductRequest(
    [property: JsonPropertyName("id")]
    Guid Id,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("category")]
    List<string> Category,
    [property: JsonPropertyName("description")]
    string? Description,
    [property: JsonPropertyName("image_file")]
    string ImageFile,
    [property: JsonPropertyName("price")]
    decimal? Price);

public record CreateProductResponse(
    [property: JsonPropertyName("id")]
    Guid Id,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("category")]
    List<string> Category,
    [property: JsonPropertyName("description")]
    string? Description,
    [property: JsonPropertyName("image_file")]
    string ImageFile,
    [property: JsonPropertyName("price")]
    decimal? Price);