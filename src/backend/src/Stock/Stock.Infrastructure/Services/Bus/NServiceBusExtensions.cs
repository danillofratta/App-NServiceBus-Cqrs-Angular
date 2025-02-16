using Microsoft.Extensions.DependencyInjection;
using Stock.Infrasctructure.Services.Bus.Endpoint;

namespace Stock.Infrasctructure.Services.Bus.Config
{
    public static class NServiceBusExtensions
    {
        public static void AddNServiceBus(this IServiceCollection services)
        {
            var nServiceBusInitializer = new StockEndPoint();
            nServiceBusInitializer.Initialize().GetAwaiter().GetResult();

            services.AddSingleton(nServiceBusInitializer.GetEndpointInstance());
            services.AddSingleton<IMessageSession>(nServiceBusInitializer.GetEndpointInstance());
        }
    }
}
