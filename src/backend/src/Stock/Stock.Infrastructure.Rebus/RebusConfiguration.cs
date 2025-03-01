using Base.Infrastructure.Messaging;
using Base.Infrastruture.Messaging.Rebus;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Rebus.Sagas.Conflicts;
using Sale.Core.Domain.Contracts.Event;
using Stock.Core.Domain.Application.Stock.Handlers;
using Stock.Core.Domain.Services;

namespace Stock.Infrastructure.Rebus
{
    public class RebusProvider : IMessageBusProvider
    {
        public void Configure(IServiceCollection services, string messagingType)
        {
            if (messagingType.ToLower() != "rebus") return;

            services.AddRebus(configure => configure
#if DEBUG
                .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost", "StockSagaEndpoint"))
#else            
                .Transport(t => t.UseRabbitMq("amqp://guest:guest@rabbitmq", "StockSagaEndpoint"))     
#endif
                .Routing(r => r.TypeBased()                
                    .Map<StockConfirmedEvent>("SaleSagaEndpoint")
                    .Map<StockInsufficientEvent>("SaleSagaEndpoint")
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
            services.AddScoped<CheckItemInStockService>();
            services.AddScoped<CalculateStockService>();
            services.AddRebusHandler<ReserveStockCommandHandle>();            

            services.AddSingleton<IMessageBus>(sp => new RebusAdapter(sp.GetRequiredService<IBus>(), sp));
        }
    }
}
