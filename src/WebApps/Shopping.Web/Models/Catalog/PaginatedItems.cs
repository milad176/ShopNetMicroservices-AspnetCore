using System.Text.Json.Serialization;

namespace Shopping.Web.Models.Catalog;

public class PaginatedItems<T>
{
    [JsonPropertyName("page_index")]
    public int PageIndex { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("count")]
    public long Count { get; set; }

    [JsonPropertyName("data")]
    public IEnumerable<T> Data { get; set; } = new List<T>();
}