using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Models;

public record PaginationRequest(
    [FromQuery(Name = "page_size")]
    int PageSize = 10,
    [FromQuery( Name = "page_index")]
    int PageIndex = 0);
