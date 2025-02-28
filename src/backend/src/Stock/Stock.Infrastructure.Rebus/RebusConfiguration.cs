using Base.Infrastructure.Messaging;
using Base.Infrastruture.Messaging.Rebus;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
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
                .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost", "SaleSagaEndpoint"))
#else            
                .Transport(t => t.UseRabbitMq("amqp://guest:guest@rabbitmq", "SaleSagaEndpoint"))     
#endif
                .Routing(r => r.TypeBased()
                    .Map<StockConfirmedEvent>("SaleSagaEndpoint")
                    .Map<StockInsufficientEvent>("SaleSagaEndpoint")
                )
                .Sagas(s => s.StoreInPostgres(
                    "Host=localhost;Port=5432;Username=admin;Password=root;Database=apitest;",
                    "SagaDataRebus",
                    "SagaDataRebusIndex"
                ))
            );
            services.AddScoped<CheckItemInStockService>();
            services.AddScoped<CalculateStockService>();
            services.AddRebusHandler<ReserveStockCommandHandle>();            

            services.AddSingleton<IMessageBus>(sp => new RebusAdapter(sp.GetRequiredService<IBus>(), sp));
        }
    }
}
