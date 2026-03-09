using BuildingBlocks.Messaging.Events;
using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler(ILogger<BasketCheckoutEventHandler> logger, ISender sender)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        // Create new order and start order fullfillment process.
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

        var command = MapToCreateOrderCommand(context.Message);
        await sender.Send(command);
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        // Create full order with incoming event data
        var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine,
            message.Country, message.State, message.ZipCode);

        var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV,
            message.PaymentMethod);

        var orderId = Guid.NewGuid();

        var orderDto = new OrderDto(
            Id: orderId,
            CustomerId: message.CustomerId,
            OrderName: message.UserName,
            ShippingAddress: addressDto,
            BillingAddress: addressDto,
            Payment: paymentDto,
            Status: Ordering.Domain.Enums.OrderStatus.Pending,
            OrderItems:
            [
                new OrderItemDto(orderId, new Guid("3ed7c32e-9be7-4a9d-929b-955b9ca19efc"), 2, 500),
                new OrderItemDto(orderId, new Guid("e3023cdf-935a-4b2c-8932-6f92eff25ff6"), 1, 400)
            ]);

        return new CreateOrderCommand(orderDto);
    }
}