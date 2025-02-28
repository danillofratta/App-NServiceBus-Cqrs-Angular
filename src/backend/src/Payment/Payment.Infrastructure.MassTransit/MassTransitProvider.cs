using Base.Infrastructure.Messaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Payment.Core.Domain.Application.Payment.Handlers;
using Sale.Core.Domain.Contracts.Command;

namespace Payment.Infrastructure.MassTransit;

public class MassTransitProvider : IMessageBusProvider
{
    public void Configure(IServiceCollection services, string messagingType)
    {
        if (messagingType.ToLower() != "masstransit") return;

        services.AddScoped<Payment.Core.Domain.Services.PaymentCreateService>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProcessPaymentHandler>();

            x.UsingRabbitMq((context, cfg) =>
            {
#if DEBUG
                cfg.Host(new Uri("amqp://guest:guest@localhost:5672/"));
#else
                cfg.Host(new Uri("amqp://guest:guest@rabbitmq:5672/"));                    
#endif

                cfg.ReceiveEndpoint("PaymentSagaEndpoint", e =>
                {
                    e.ConfigureConsumer<ProcessPaymentHandler>(context);
                });

                cfg.ConfigureEndpoints(context);
                cfg.UseRawJsonSerializer();
            });
        });

        services.AddMassTransitHostedService();
        services.AddScoped<IMessageBus, MassTransitAdapter>();
    }
}
