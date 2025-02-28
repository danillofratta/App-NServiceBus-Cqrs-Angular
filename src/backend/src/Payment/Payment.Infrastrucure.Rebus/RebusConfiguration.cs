using Base.Infrastructure.Messaging;
using Base.Infrastruture.Messaging.Rebus;
using Microsoft.Extensions.DependencyInjection;
using Payment.Core.Domain.Application.Payment.Handlers;
using Payment.Core.Domain.Services;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Sale.Core.Domain.Contracts.Event;

namespace Payment.Infrastructure.Rebus
{
    public class RebusProvider : IMessageBusProvider
    {
        public void Configure(IServiceCollection services, string messagingType)
        {
            if (messagingType.ToLower() != "rebus") return;

            services.AddRebus(configure => configure
#if DEBUG
                .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost", "SaleSagaEndpoint"))
#else            
                .Transport(t => t.UseRabbitMq("amqp://guest:guest@rabbitmq", "SaleSagaEndpoint"))     
#endif
                .Routing(r => r.TypeBased()
                    .Map<PaymentConfirmedEvent>("SaleSagaEndpoint")
                    .Map<PaymentFailEvent>("SaleSagaEndpoint")
                )
                .Sagas(s => s.StoreInPostgres(
                    "Host=localhost;Port=5432;Username=admin;Password=root;Database=apitest;",
                    "SagaDataRebus",
                    "SagaDataRebusIndex"
                ))
            );

            services.AddScoped<PaymentCreateService>();
            services.AddRebusHandler<ProcessPaymentHandler>();


            services.AddSingleton<IMessageBus>(sp => new RebusAdapter(sp.GetRequiredService<IBus>(), sp));
        }
    }
}
