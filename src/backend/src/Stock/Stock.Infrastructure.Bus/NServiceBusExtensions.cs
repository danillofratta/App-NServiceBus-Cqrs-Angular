using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Sale.Core.Domain.Saga.Commands;
using Sale.Core.Domain.Saga.Events;
using Sale.Core.Domain.Service;
using Shared.Infrastructure;
using Stock.Core.Domain.Application.Stock.Handlers;
using Stock.Core.Domain.Repository;
using Stock.Core.Domain.Services;
using Stock.Infrastructure.Orm.Repository;

namespace Stock.Infrasctructure.Services.Bus
{
    public static class NServiceBusExtensions
    {
        public static void AddNServiceBus(this IServiceCollection services)
        {
            var sagaEndpoint = ConfigureSagaEndpoint(services);
            var sagaEndpointInstance = sagaEndpoint.GetAwaiter().GetResult();
            services.AddSingleton(sagaEndpointInstance);
            services.AddSingleton<IMessageSession>(sagaEndpointInstance);
        }

        private static async Task<IEndpointInstance> ConfigureSagaEndpoint(IServiceCollection services)
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<IServiceCollection>>();
            await Task.Delay(TimeSpan.FromSeconds(4));

            var endpointConfiguration = new EndpointConfiguration("StockSagaEndpoint");

            // Transport Configuration
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology(QueueType.Classic);
            //todo put in appsettings
            //todo put in appsettings
            #if DEBUG
                        transport.ConnectionString("amqp://guest:guest@localhost:5672/");
                        logger.LogInformation("Iniciando configuração do NServiceBus: STOCK DEBUG");
            #else
            transport.ConnectionString("amqp://guest:guest@rabbitmq:5672/");
            logger.LogInformation("Iniciando configuração do NServiceBus: STOCK RELEASE");
                        Console.Write("RELEASE");
            #endif
            // Message Routing
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(StockConfirmedEvent), "SaleSagaEndpoint"); 
            routing.RouteToEndpoint(typeof(StockInsufficientEvent), "SaleSagaEndpoint");            

            // Error Handling
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(immediate => immediate.NumberOfRetries(2));
            recoverability.Delayed(delayed => delayed.NumberOfRetries(1).TimeIncrease(TimeSpan.FromSeconds(10)));

            // Persistence
            endpointConfiguration.UsePersistence<LearningPersistence>();

            // Component Registration
            endpointConfiguration.RegisterComponents(registration =>
            {
                registration.AddLogging();
                registration.AddDbContext<DefaultDbContext>();
                registration.AddScoped<IStockRepository, StockRepository>();
                registration.AddTransient<SaleStockInsufficientService>();
                registration.AddTransient<CalculateStockService>();
                registration.AddTransient<CheckItemInStockService>();
                registration.AddTransient<IHandleMessages<ReserveStockCommand>, ReserveStockCommandHandle>();
                
            });

            return await NServiceBus.Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        }        
    }
}
