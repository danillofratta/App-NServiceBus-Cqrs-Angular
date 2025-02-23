using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;
using Sale.Core.Application.Sales.Create;
using Sale.Core.Domain.Application.Sale.Handle;
using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Service;
using Sale.Infrastructure.Orm.Repository;
using Shared.Infrastructure;

using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

namespace Sale.Infrasctructure.Services.Bus
{
    public static class NServiceBusConfiguration
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
            await Task.Delay(TimeSpan.FromSeconds(3));

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
            Console.Write("RELEASE");
#endif

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningEventsAs(type => type.Namespace != null && type.Namespace.Contains(".Events"));            

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(immediate => immediate.NumberOfRetries(2)); // Reduzir tentativas imediatas
            recoverability.Delayed(delayed => delayed.NumberOfRetries(1).TimeIncrease(TimeSpan.FromSeconds(20))); // Reduzir retries automáticos

            // Persistence
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var connection = "Host=localhost;Port=5432;Username=admin;Password=root;Database=apitest;";

            persistence.ConnectionBuilder(() => new NpgsqlConnection(connection));
            persistence.SqlDialect<SqlDialect.PostgreSql>();            
            var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
            dialect.JsonBParameterModifier(parameter =>
            {
                var npgsqlParameter = (NpgsqlParameter)parameter;
                npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
            });

            persistence.TablePrefix("");
            endpointConfiguration.EnableInstallers();

            endpointConfiguration.EnableOutbox();
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);  
            
            endpointConfiguration.EnableOpenTelemetry();
            services.AddOpenTelemetry()
                       .WithTracing(tracerProviderBuilder =>
                       {
                           tracerProviderBuilder
                               .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("SaleSaga"))
                               .AddAspNetCoreInstrumentation()
                               .AddHttpClientInstrumentation();
                               //.AddNServiceBusInstrumentation() // Captura telemetria do NServiceBus
                               //.AddOtlpExporter(options =>
                               //{
                               //    options.Endpoint = new Uri("http://localhost:4317"); // OTLP para Jaeger ou outro observability backend
                               //})
                               //.AddConsoleExporter();
                       })
                       .WithMetrics(metricsProviderBuilder =>
                       {
                           metricsProviderBuilder
                               .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("SaleSaga"))
                               .AddAspNetCoreInstrumentation()
                               .AddHttpClientInstrumentation()
                               .AddMeter("NServiceBus") // Para métricas do NServiceBus
                               .AddConsoleExporter();
                       });

            // Component Registration
            endpointConfiguration.RegisterComponents(registration =>
            {
                registration.AddLogging();

                registration.AddDbContext<DefaultDbContext>();
                registration.AddScoped<ISaleRepository, SaleRepository>();

                registration.AddTransient<CreateSaleHandler>();

                registration.AddTransient<SaleStockConfirmedService>();
                registration.AddTransient<SalePaymentFailedService>();
                registration.AddTransient<SalePaymentConfirmedService>();
                registration.AddTransient<SaleStockInsufficientService>();

                registration.AddTransient<SalePaymentCanceleldHandle>();
                registration.AddTransient<SalePaymentConfirmedHandle>();
                registration.AddTransient<SaleStockConfirmedHandle>();
                registration.AddTransient<SaleStockInsufficienHandle>();
            });

            return await NServiceBus.Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        }
    }
}
