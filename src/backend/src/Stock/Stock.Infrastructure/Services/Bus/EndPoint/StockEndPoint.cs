using Shared.Dto.Event.Sale;
using Microsoft.Extensions.DependencyInjection;
using Stock.Core.Domain.Services;
using Stock.Core.Domain.Repository;
using Stock.Infrastructure.Repository;
using Shared.Infrastructure;
using Stock.Core.Application;

namespace Stock.Infrasctructure.Services.Bus.Endpoint;

public class StockEndPoint
{
    private IEndpointInstance _endpointInstance;

    public IEndpointInstance GetEndpointInstance() => _endpointInstance;

    public async Task Initialize()
    {
        var endpointConfiguration = new NServiceBus.EndpointConfiguration("stock.webapi");

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        var rabbitMqTransport = transport.UseConventionalRoutingTopology(QueueType.Classic);
        rabbitMqTransport.ConnectionString("amqp://guest:guest@localhost:5672/");        

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
        //create sale and send to stock
        routing.RouteToEndpoint(typeof(CreateSaleEventDto), "stock.webapi");
        //stock check and send to sale
        routing.RouteToEndpoint(typeof(CreateSaleEventReponseFromStockDto), "sale.webapi");
    }

    /// <summary>
    /// Register injection
    /// </summary>
    /// <param name="endpointConfiguration"></param>
    private void RegisterComponent(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.AddLogging();

                components.AddDbContext<DefaultDbContext>();

                components.AddScoped<IStockRepository, StockRepository>();

                components.AddScoped<SubtractAndCheckItemInStockService>();

                components.AddAutoMapper(typeof(StockEndPoint).Assembly);

                components.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(
                        typeof(ApplicationLayer).Assembly
                    );
                });
            });
    }

}