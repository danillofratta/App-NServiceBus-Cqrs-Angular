using Microsoft.Extensions.Hosting;
using Sale.Infrasctructure.Services.Bus;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddNServiceBus(); // Configuração do NServiceBus
                                           //services.AddApplicationServices(); // Registra handlers e casos de uso
            })
            .Build();

        await host.RunAsync();
    }
}