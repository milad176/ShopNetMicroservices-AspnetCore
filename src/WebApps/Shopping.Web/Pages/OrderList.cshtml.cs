using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping.Web.Pages;

public class OrderListModel(IOrderingService orderingService, ILogger<OrderListModel> logger) : PageModel
{
    public IEnumerable<OrderModel> Orders { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        // assumption customerId is passed in from the UI authenticated user swn
        var customerId = new Guid("28d53fad-ed8a-4e78-8e71-f2004b4e1158");

        var response = await orderingService.GetOrdersByCustomer(customerId);
        Orders = response.Orders;

        return Page();
    }
}