using System.Text.Json.Serialization;

namespace Catalog.API.Features.Products.GetProductById
{
    public record GetProductByIdResponse(
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
}
