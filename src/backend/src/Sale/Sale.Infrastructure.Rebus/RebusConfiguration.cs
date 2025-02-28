using Sale.Core.Domain.Contracts.Command;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Sale.Core.Domain.Contracts.Event;
using Sale.Core.Domain.Application.Sale.Handle;
using Sale.Core.Domain.Service;
using Sale.Core.Domain.SagaRebus;
using Base.Infrastruture.Messaging.Rebus;
using Rebus.Bus;
using Base.Infrastructure.Messaging;
using static MassTransit.Logging.LogCategoryName;

namespace Sale.Infrastructure.Rebus
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
                    .Map<ReserveStockCommand>("StockSagaEndpoint")
                    .Map<ProcessPaymentCommand>("PaymentSagaEndpoint")                         
                    .Map<SaleStockConfirmedCommand>("SaleSagaEndpoint")
                    .Map<SaleStockInsufficienCommand>("SaleSagaEndpoint")
                    .Map<SalePaymentConfirmedCommand>("SaleSagaEndpoint")
                    .Map<SalePaymentCancelledCommand>("SaleSagaEndpoint")
                )
                .Sagas(s => s.StoreInPostgres(
                    "Host=localhost;Port=5432;Username=admin;Password=root;Database=apitest;",
                    "SagaDataRebus",
                    "SagaDataRebusIndex"
                ))
            );

            services.AutoRegisterHandlersFromAssemblyOf<SaleSaga>();
            services.AddRebusHandler<SalePaymentCancelledHandle>();
            services.AddRebusHandler<SalePaymentConfirmedHandle>();
            services.AddRebusHandler<SaleStockConfirmedHandle>();
            services.AddRebusHandler<SaleStockInsufficienHandle>();

            services.AddScoped<SalePaymentFailedService>();
            services.AddScoped<SalePaymentConfirmedService>();
            services.AddScoped<SaleStockConfirmedService>();
            services.AddScoped<SaleStockInsufficientService>();

            services.AddSingleton<IMessageBus>(sp => new RebusAdapter(sp.GetRequiredService<IBus>(), sp));
        }
    }
}
