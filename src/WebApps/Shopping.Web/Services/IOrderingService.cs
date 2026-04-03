namespace Shopping.Web.Services;

public interface IOrderingService
{
    [Get("/ordering-service/api/v1/orders?pageIndex={pageIndex}&pageSize={pageSize}\"")]
    Task<GetOrdersResponse> GetOrders(int? pageIndex = 1, int? pageSize = 10);

    [Get("/ordering-service/api/v1/orders/{orderName}")]
    Task<GetOrdersByNameResponse> GetOrdersByName(string orderName);

    [Get("/ordering-service/api/v1/orders/customer/{customerId}")]
    Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(Guid customerId);
}