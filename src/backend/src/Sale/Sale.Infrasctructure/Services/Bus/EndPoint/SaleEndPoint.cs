﻿using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Routing;
using Sale.Core.Application;
using Sale.Core.Application.Sale.Commands.CreateSale.Event;
using Sale.Core.Domain.Repository;
using Sale.Infrastructure.Repository;
using Shared.Dto.Event.Sale;
using Shared.Infrastructure;
using Stock.Core.Domain.Repository;

namespace Sale.Infrasctructure.Services.Bus.Endpoint;

public class SaleEndPoint
{
    private IEndpointInstance _endpointInstance;

    public IEndpointInstance GetEndpointInstance() => _endpointInstance;

    public async Task Initialize()
    {
        var endpointConfiguration = new NServiceBus.EndpointConfiguration("sale.webapi");

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        var rabbitMqTransport = transport.UseConventionalRoutingTopology(QueueType.Classic);
        rabbitMqTransport.ConnectionString("amqp://guest:guest@localhost:5672/");

        this.CreateRouting(transport);

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(immediate => immediate.NumberOfRetries(2));
        recoverability.Delayed(delayed => delayed.NumberOfRetries(1).TimeIncrease(TimeSpan.FromSeconds(10)));

        endpointConfiguration.UsePersistence<LearningPersistence>();

        this.CreateRouting(transport);
        this.RegisterComponent(endpointConfiguration);

        try
        {
            _endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            //Log.Error(ex, "Failed to start NServiceBus endpoint");
            throw;
        }
    }

    private void CreateRouting(TransportExtensions<RabbitMQTransport> transport)
    {        
        var routing = transport.Routing();

        routing.RouteToEndpoint(typeof(CreateSaleEventDto), "stock.webapi");

    }

    private void RegisterComponent(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.AddLogging();

                components.AddDbContext<DefaultDbContext>();

                components.AddScoped<ISaleRepository, SaleRepository>();
                components.AddScoped<CreateSaleHandlerEventFromStock>();                

                components.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(
                        typeof(ApplicationLayer).Assembly
                    );
                });
            });
    }

}