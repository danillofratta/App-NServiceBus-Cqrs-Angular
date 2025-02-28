using Base.Infrastructure.Messaging;
using Base.Infrastructure.Messaging.NServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using NServiceBus.Logging;
using Sale.Core.Domain.Application.Sale.Handle;
using Sale.Core.Domain.Contracts.Command;
using Sale.Core.Domain.Contracts.Event;
using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Service;
using Sale.Infrastructure.Orm.Repository;
using Shared.Infrastructure.Orm;
using LogLevel = NServiceBus.Logging.LogLevel;

namespace Sale.Infrastructure.NServiceBus
{
    public class NServiceBusProvider : IMessageBusProvider
    {
        public void Configure(IServiceCollection services, string messagingType)
        {
            if (messagingType.ToLower() != "nservicebus") return;

            var sagaEndpoint = ConfigureSagaEndpoint(services);
            var sagaEndpointInstance = sagaEndpoint.GetAwaiter().GetResult();


            services.AddSingleton(sagaEndpointInstance);

            services.AddSingleton<IMessageSession>(sp => sp.GetRequiredService<IEndpointInstance>());

            services.AddSingleton<IMessageBus>(sp => new NServiceBusAdapter(sp.GetRequiredService<IMessageSession>()));
            
        }
        private static async Task<IEndpointInstance> ConfigureSagaEndpoint(IServiceCollection services)
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<IServiceCollection>>();
            await Task.Delay(TimeSpan.FromSeconds(3));

            LogManager.Use<DefaultFactory>().Level(LogLevel.Debug);
            var endpointConfiguration = new EndpointConfiguration("SaleSagaEndpoint");

            // Transport Configuration
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology(QueueType.Classic);

            //todo put in appsettings
#if DEBUG
            transport.ConnectionString("amqp://guest:guest@localhost:5672/");
            logger.LogInformation("Iniciando configuração do NServiceBus: PAYMENT DEBUG");
#else
            transport.ConnectionString("amqp://guest:guest@rabbitmq:5672/");       
            logger.LogInformation("Iniciando configuração do NServiceBus: PAYMENT RELEASE");
#endif
            // Persistence
            var connection = "Host=localhost;Port=5432;Username=admin;Password=root;Database=apitest;";
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1))
                ;
            persistence.SqlDialect<SqlDialect.PostgreSql>();
            var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
            dialect.JsonBParameterModifier(
                modifier: parameter =>
                {
                    var npgsqlParameter = (NpgsqlParameter)parameter;
                    npgsqlParameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb;
                });
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new NpgsqlConnection(connection);
                });

            persistence.TablePrefix("");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableOutbox();
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningEventsAs(type => type.Namespace != null && (type.Namespace.Contains("Event") || type.Name.EndsWith("Event")));
            conventions.DefiningCommandsAs(type => type.Namespace != null && type.Name.EndsWith("Command"));

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(ReserveStockCommand), "StockSagaEndpoint");
            routing.RouteToEndpoint(typeof(SaleCreatedEvent), "SaleSagaEndpoint");           

            // Component Registration
            endpointConfiguration.RegisterComponents(registration =>
            {
                registration.AddLogging();

                registration.AddDbContext<DefaultDbContext>();
                registration.AddScoped<ISaleRepository, SaleRepository>();

                registration.AddTransient<SaleStockConfirmedService>();
                registration.AddTransient<SalePaymentFailedService>();
                registration.AddTransient<SalePaymentConfirmedService>();
                registration.AddTransient<SaleStockInsufficientService>();

                registration.AddTransient<SalePaymentCancelledHandle>();
                registration.AddTransient<SalePaymentConfirmedHandle>();
                registration.AddTransient<SaleStockConfirmedHandle>();
                registration.AddTransient<SaleStockInsufficienHandle>();

                registration.AddSingleton<IEndpointInstance>(sp => sp.GetRequiredService<IEndpointInstance>());
                registration.AddSingleton<IMessageSession>(sp => sp.GetRequiredService<IEndpointInstance>());
                registration.AddSingleton<IMessageBus>(sp => new NServiceBusAdapter(sp.GetRequiredService<IMessageSession>()));
            });


            var endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            //endpointConfiguration.RegisterComponents(registration =>
            //{
            //    registration.AddSingleton<IMessageBus>(sp => new NServiceBusAdapter(sp.GetRequiredService<IMessageSession>()));

            //    registration.AddSingleton<IMessageSession>(sp => sp.GetRequiredService<IEndpointInstance>());


            //    registration.AddSingleton<SaleSaga>();
            //    //            registration.AddSingleton(typeof(SaleSaga),
            //    //context => new SaleSaga(
            //    //    context.GetService<IMessageBus>(),
            //    //    context.GetService<ILogger<SaleSaga>>()));
            //});


            return endpoint;
        }
    }
}
