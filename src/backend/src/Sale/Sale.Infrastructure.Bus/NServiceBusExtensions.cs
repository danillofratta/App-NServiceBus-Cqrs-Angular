using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Saga;
using Sale.Core.Domain.Saga.Commands;
using Sale.Core.Domain.Service;
using Sale.Infrastructure.Orm.Repository;
using Shared.Infrastructure;
using System;
using System.ComponentModel;

namespace Sale.Infrasctructure.Services.Bus
{
    public static class NServiceBusExtensions
    {
        public static void AddNServiceBus(this IServiceCollection services)
        {
            var sagaEndpoint = ConfigureSagaEndpoint();
            var sagaEndpointInstance = sagaEndpoint.GetAwaiter().GetResult();
            
            services.AddSingleton(sagaEndpointInstance);
            services.AddSingleton<IMessageSession>(sagaEndpointInstance);
        }

        private static async Task<IEndpointInstance> ConfigureSagaEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration("SaleSagaEndpoint");
            
            // Transport Configuration
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology(QueueType.Classic);
            transport.ConnectionString("amqp://guest:guest@localhost:5672/");

            //transport.Routing(). EnableMessageDrivenPubSub();
            transport.PrefetchCount(1);

            // Message Routing
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(ReserveStockCommand), "StockSagaEndpoint");
            //routing.RouteToEndpoint(typeof(ProcessPaymentCommand), "PaymentSagaEndpoint");

            // Error Handling
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(immediate => immediate.NumberOfRetries(1)); // Reduzir tentativas imediatas
            recoverability.Delayed(delayed => delayed.NumberOfRetries(1).TimeIncrease(TimeSpan.FromSeconds(5))); // Reduzir retries automáticos

            // Persistence
            endpointConfiguration.UsePersistence<LearningPersistence>();           

            // Component Registration
            endpointConfiguration.RegisterComponents(registration =>
            {
                registration.AddLogging();
                
                registration.AddDbContext<DefaultDbContext>();
                registration.AddScoped<ISaleRepository, SaleRepository>();

                //if use singleton error in trackrecord sale need commit or something like this
                registration.AddScoped<SaleSaga>();

                registration.AddScoped<SaleStockConfirmedService>();
                registration.AddScoped<SaleStockInsufficientService>();

                registration.AddScoped<SalePaymentFailedService>();
                registration.AddScoped<SalePaymentConfirmedService>();
            });

            return await NServiceBus.Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        }
    }
}
