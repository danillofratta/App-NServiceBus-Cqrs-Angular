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
using Microsoft.Extensions.Hosting;
using Rebus.Sagas.Conflicts;
using Rebus.Retry.Simple;
using Rebus.Persistence.InMem;

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
            );;

            services.AutoRegisterHandlersFromAssemblyOf<SaleSaga>();
            services.AddRebusHandler<SalePaymentCancelledHandle>();
            services.AddRebusHandler<SalePaymentConfirmedHandle>();
            services.AddRebusHandler<SaleStockConfirmedHandle>();
            services.AddRebusHandler<SaleStockInsufficienHandle>();

            //services.AutoRegisterHandlersFromAssemblyOf<SalePaymentCancelledHandle>();
            //services.AutoRegisterHandlersFromAssemblyOf<SalePaymentConfirmedHandle>();
            //services.AutoRegisterHandlersFromAssemblyOf<SaleStockConfirmedHandle>();
            //services.AutoRegisterHandlersFromAssemblyOf<SaleStockInsufficienHandle>();

            services.AddScoped<SalePaymentFailedService>();
            services.AddScoped<SalePaymentConfirmedService>();
            services.AddScoped<SaleStockConfirmedService>();
            services.AddScoped<SaleStockInsufficientService>();

            services.AddSingleton<IMessageBus>(sp => new RebusAdapter(sp.GetRequiredService<IBus>(), sp));

            // Adiciona o serviço de subscrição
            services.AddHostedService<RebusSubscriptionService>();
        }
    }

    // Serviço para realizar a subscrição
    public class RebusSubscriptionService : IHostedService
    {
        private readonly IBus _bus;

        public RebusSubscriptionService(IBus bus)
        {
            _bus = bus;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _bus.Subscribe<SaleCreatedEvent>();
            await _bus.Subscribe<StockConfirmedEvent>();
            await _bus.Subscribe<StockInsufficientEvent>();
            await _bus.Subscribe<PaymentFailEvent>();
            await _bus.Subscribe<ProcessPaymentEvent>();
            await _bus.Subscribe<PaymentConfirmedEvent>();
            

            Console.WriteLine("SaleSagaEndpoint inscrito no SaleCreatedEvent"); // Ou use ILogger
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Opcional: cancelar a subscrição ao encerrar
            await _bus.Unsubscribe<SaleCreatedEvent>();
            //await _bus.Unsubscribe<StockConfirmedEvent>();
            await _bus.Unsubscribe<StockInsufficientEvent>();
            await _bus.Unsubscribe<PaymentFailEvent>();
            await _bus.Unsubscribe<ProcessPaymentEvent>();            
            Console.WriteLine("Subscrição do SaleCreatedEvent cancelada");
        }
    }
}
