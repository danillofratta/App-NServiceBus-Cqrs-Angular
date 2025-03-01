using Base.Infrastructure.Messaging;
using Base.Infrastruture.Messaging.Rebus;
using Microsoft.Extensions.DependencyInjection;
using Payment.Core.Domain.Application.Payment.Handlers;
using Payment.Core.Domain.Services;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Rebus.Sagas.Conflicts;
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
                .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost", "PaymentSagaEndpoint"))
#else            
                .Transport(t => t.UseRabbitMq("amqp://guest:guest@rabbitmq", "PaymentSagaEndpoint"))     
#endif
                .Routing(r => r.TypeBased()
                    .Map<PaymentConfirmedEvent>("SaleSagaEndpoint")
                    .Map<PaymentFailEvent>("SaleSagaEndpoint")
                )
                .Sagas(s => s.SetMaxConflictResolutionAttempts(1))
                //.Sagas(s => s.StoreInMemory())
                .Sagas(s => s.StoreInPostgres(
                    "Host=localhost;Port=5432;Username=admin;Password=root;Database=apitest;",
                    "SagaDataRebus",
                    "SagaDataRebusIndex"
                ))
                .Logging(l => l.Trace())
                .Options(o => o.SetNumberOfWorkers(1))
                .Options(o => o.SetMaxParallelism(1))
                .Options(o => o.RetryStrategy(errorQueueName: "errors", maxDeliveryAttempts: 5))
            ); 

            services.AddScoped<PaymentCreateService>();
            services.AddRebusHandler<ProcessPaymentHandler>();

            services.AddSingleton<IMessageBus>(sp => new RebusAdapter(sp.GetRequiredService<IBus>(), sp));
        }
    }
}
