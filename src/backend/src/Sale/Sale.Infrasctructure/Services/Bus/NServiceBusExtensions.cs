using Microsoft.Extensions.DependencyInjection;
using Sale.Infrasctructure.Services.Bus.Endpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Infrasctructure.Services.Bus.Config
{
    public static class NServiceBusExtensions
    {
        public static void AddNServiceBus(this IServiceCollection services)
        {
            var nServiceBusInitializer = new SaleEndPoint();
            nServiceBusInitializer.Initialize().GetAwaiter().GetResult();

            services.AddSingleton(nServiceBusInitializer.GetEndpointInstance());
            services.AddSingleton<IMessageSession>(nServiceBusInitializer.GetEndpointInstance());            
        }
    }
}
