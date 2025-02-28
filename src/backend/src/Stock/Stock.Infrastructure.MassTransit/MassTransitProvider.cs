using Base.Infrastructure.Messaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Stock.Core.Domain.Application.Stock.Handlers;

namespace Stock.Infrastructure.MassTransit;

public class MassTransitProvider : IMessageBusProvider
{
    public void Configure(IServiceCollection services, string messagingType)
    {
        if (messagingType.ToLower() != "masstransit") return;

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ReserveStockCommandHandle>();

            x.UsingRabbitMq((context, cfg) =>
            {
#if DEBUG
                cfg.Host(new Uri("amqp://guest:guest@localhost:5672/"));
#else
                cfg.Host(new Uri("amqp://guest:guest@rabbitmq:5672/"));                    
#endif

                cfg.ReceiveEndpoint("StockSagaEndpoint", e =>
                {
                    e.ConfigureConsumer<ReserveStockCommandHandle>(context);
                });
                cfg.ConfigureEndpoints(context);
                cfg.UseRawJsonSerializer();
            });
        });

        services.AddMassTransitHostedService();
        services.AddScoped<IMessageBus, MassTransitAdapter>();
    }
}