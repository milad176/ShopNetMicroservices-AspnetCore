namespace Ordering.Infrastructure.Data.Extensions;

internal class InitialData
{
    public static IEnumerable<Customer> Customers =>
        new List<Customer>()
        {
            Customer.Create(CustomerId.Of(new Guid("66182d7e-6b58-40ed-9453-2677f00ca9fa")), "Tom", "tom@gmail.com"),
            Customer.Create(CustomerId.Of(new Guid("28d53fad-ed8a-4e78-8e71-f2004b4e1158")), "Alex", "alex@gmail.com")
        };

    public static IEnumerable<Product> Products =>
        new List<Product>()
        {
            Product.Create(ProductId.Of(new Guid("3ed7c32e-9be7-4a9d-929b-955b9ca19efc")), "IPhone X", 500),
            Product.Create(ProductId.Of(new Guid("e3023cdf-935a-4b2c-8932-6f92eff25ff6")), "Samsung 22", 400),
            Product.Create(ProductId.Of(new Guid("91bba1c5-a685-4f39-b473-37c23f38066c")), "Sony X", 650),
            Product.Create(ProductId.Of(new Guid("568483c6-732c-4b75-a934-fdc7edb5ae17")), "Nokia", 450)
        };

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address1 = Address.Of("tom", "Nilson", "tom@gmail.com", "Calista Wise 7292 Dictum Av.", "USA",
                "Virginea", "47096");
            var address2 = Address.Of("Alex", "Nilson", "alex@gmail.com", "Forrest Ray 191-103 Integer Rd.", "USA",
                "New Mexico", "08219");

            var payment1 = Payment.Of("tom", "55566677799333", "12/28", "021", 1);
            var payment2 = Payment.Of("Alex", "99988866643223", "21/30", "078", 2);

            var order1 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("66182d7e-6b58-40ed-9453-2677f00ca9fa")),
                OrderName.Of("ORD-1"),
                shippingAddress: address1,
                billingAddress: address1,
                payment1);
            order1.Add(ProductId.Of(new Guid("3ed7c32e-9be7-4a9d-929b-955b9ca19efc")), 2, 500);
            order1.Add(ProductId.Of(new Guid("e3023cdf-935a-4b2c-8932-6f92eff25ff6")), 1, 400);


            var order2 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("28d53fad-ed8a-4e78-8e71-f2004b4e1158")),
                OrderName.Of("ORD-2"),
                shippingAddress: address2,
                billingAddress: address2,
                payment2);
            order2.Add(ProductId.Of(new Guid("91bba1c5-a685-4f39-b473-37c23f38066c")), 2, 650);
            order2.Add(ProductId.Of(new Guid("568483c6-732c-4b75-a934-fdc7edb5ae17")), 1, 450);

            return new List<Order> { order1, order2 };
        }
    }
}