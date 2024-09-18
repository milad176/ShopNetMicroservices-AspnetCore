using Marten.Schema;
using Polly;
using Serilog;
using System.Net.Sockets;


namespace Catalog.API.Data;

public class CatalogInitialDataMigration : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        var logger = Log.Logger.ForContext<CatalogInitialDataMigration>();

        try
        {
            logger.Information("Migrate postresql database started.");
            var policy = Policy.Handle<SocketException>()
               .Or<Marten.Exceptions.MartenCommandException>()
               .WaitAndRetryAsync(retryCount: 7, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
               {
                   logger.Error(ex, "An error occurred while migrating the postresql database, failed trying after {TimeOut}s", $"{time.TotalSeconds:n1}");
               });

            await policy.ExecuteAsync(async () =>
            {
                await using var session = store.LightweightSession();
                if (await session.Query<Product>().AnyAsync(token: cancellation))
                    return;

                session.Store<Product>(GetPreConfiguredProducts());
                await session.SaveChangesAsync(cancellation);
            });
        }
        catch (Marten.Exceptions.MartenCommandException ex)
        {
            logger.Error(ex, "An error occurred while migrating the postresql database.");
        }
    }

    private static IEnumerable<Product> GetPreConfiguredProducts() =>
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = "IPhone X",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-1.png",
            Price = 950.00M,
            Category = ["Smart Phone"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Samsung 10",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-2.png",
            Price = 840.00M,
            Category = ["Smart Phone"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Huawei Plus",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-3.png",
            Price = 650.00M,
            Category = ["White Appliances"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Xiaomi Mi 9",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-4.png",
            Price = 470.00M,
            Category = ["White Appliances"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "HTC U11+ Plus",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-5.png",
            Price = 380.00M,
            Category = ["Smart Phone"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "LG G7 ThinQ",
            Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            ImageFile = "product-6.png",
            Price = 240.00M,
            Category = ["Home Kitchen"]
        }
    ];
}
