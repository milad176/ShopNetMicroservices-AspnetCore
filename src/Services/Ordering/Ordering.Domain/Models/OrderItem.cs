namespace Ordering.Domain.Models;

public class OrderItem : Entity<Guid>
{
    public OrderItem(Guid orderId, Guid productId, int quantity, decimal unitPrice)
    {
    }

    public Guid OrderId { get; set; } = default!;
    public Guid ProductId { get; set; } = default!;
    public int Quantity { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}