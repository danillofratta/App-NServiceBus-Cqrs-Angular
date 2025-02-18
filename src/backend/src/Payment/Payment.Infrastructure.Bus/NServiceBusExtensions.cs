using Microsoft.Extensions.DependencyInjection;
using Payment.Core.Domain.Application.Handlers;
using Payment.Core.Domain.Repository;
using Payment.Core.Domain.Services;
using Payment.Infrastructure.Orm.Repository;
using Sale.Core.Domain.Saga.Commands;
using Sale.Core.Domain.Saga.Events;
using Shared.Infrastructure;


namespace Payment.Infrasctructure.Services.Bus
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
            var endpointConfiguration = new EndpointConfiguration("PaymentSagaEndpoint");

            // Transport Configuration
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology(QueueType.Classic);
            transport.ConnectionString("amqp://guest:guest@localhost:5672/");

            // Message Routing
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PaymentFailEvent), "SaleSagaEndpoint");
            routing.RouteToEndpoint(typeof(PaymentConfirmedEvent), "SaleSagaEndpoint");

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
                registration.AddScoped<IPaymentRepository, PaymentRepository>();

                registration.AddTransient<IHandleMessages<ProcessPaymentCommand>, ProcessPaymentHandler>();

                registration.AddTransient<PaymentCreateService>();
            });

            return await NServiceBus.Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        }        
    }
}
