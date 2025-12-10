using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddDbContext<OrderContext>(options =>
        // options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));

        //services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}