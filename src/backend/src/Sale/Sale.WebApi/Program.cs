using Base.Infrastructure.Messaging;
using Sale.Core.Application.Sales.Create;
using Sale.Core.Domain.Application;
using Sale.Core.Domain.Repository;
using Sale.Infrastructure.Orm.Repository;
using Serilog;
using Shared.Infrastructure.Orm;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(new LoggerConfiguration()
        //.MinimumLevel.Information() // Ou Debug para mais detalhes
        .WriteTo.Console()
        //.WriteTo.File("logs/sale.log", rollingInterval: RollingInterval.Day)
        .WriteTo.Debug()        
        .CreateLogger());

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
    //builder.AddSerilog();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(ApplicationLayer).Assembly,
        typeof(Program).Assembly
    );
});


builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);
builder.Services.AddDbContext<DefaultDbContext>();

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<CreateSaleCommand>();
builder.Services.AddScoped<CreateSaleResult>();
builder.Services.AddScoped<CreateSaleHandler>();


// Configuração dinâmica do IMessageBus via provedores
string messagingType = builder.Configuration["Messaging:Type"] ?? "Rebus";
ConfigureMessageBus(builder.Services, messagingType);


//aqui com servicebus rodando dentro webpi
//builder.Services.AddNServiceBus();
//builder.Services.AddScoped<IMessageSession>(provider =>
//{
//    var endpointInstance = provider.GetRequiredService<IEndpointInstance>();
//    return endpointInstance;
//});

//builder.Services.AddRebusInfrastructure();
//builder.Services.AddHostedService<RebusSubscriptions>();

//aqui com servicebus rorando em outro serviço
//configura servicebus que é chamado em outro processo fora da webapi
//builder.Services.AddSingleton(provider =>
//{
//    var endpointConfiguration = new EndpointConfiguration("SaleSagaEndpoint");

//     Defina o transporte correto (exemplo: RabbitMQ, Azure Service Bus, etc.)
//    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
//    transport.UseConventionalRoutingTopology(QueueType.Classic);
//    transport.ConnectionString("amqp://guest:guest@localhost:5672/");// Troque para o transporte real, se necessário
//    endpointConfiguration.UsePersistence<LearningPersistence>();

//    endpointConfiguration.UseSerialization<SystemJsonSerializer>();

//    var endpointInstance = NServiceBus.Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
//    return endpointInstance;
//});



#if DEBUG

#else
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // Porta do container
});
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(cors => cors
.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin()
);

app.UseCors((g) => g.AllowAnyOrigin());

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapGet("/", () => "NServiceBus está rodando!");

app.Run();

static void ConfigureMessageBus(IServiceCollection services, string messagingType)
{
    string providerNamespace = $"Sale.Infrastructure.{messagingType}";
    string providerFullName = $"{providerNamespace}.{messagingType}Provider";

    try
    {
        Console.WriteLine($"Tentando carregar assembly: {providerNamespace}");
        var assembly = Assembly.Load($"{providerNamespace}");
        Console.WriteLine($"Assembly carregado com sucesso: {assembly.FullName}");

        var providerType = assembly.GetType(providerFullName);
        if (providerType == null)
        {
            Console.WriteLine($"Tipo {providerFullName} não encontrado no assembly.");
            throw new InvalidOperationException($"Provedor para {messagingType} não encontrado ou inválido.");
        }
        if (!typeof(IMessageBusProvider).IsAssignableFrom(providerType))
        {
            Console.WriteLine($"Tipo {providerFullName} não implementa IMessageBusProvider.");
            throw new InvalidOperationException($"Provedor para {messagingType} não encontrado ou inválido.");
        }

        Console.WriteLine($"Criando instância de {providerFullName}");
        var provider = Activator.CreateInstance(providerType) as IMessageBusProvider;
        if (provider == null)
        {
            throw new InvalidOperationException($"Não foi possível criar instância do provedor para {messagingType}.");
        }
        provider?.Configure(services, messagingType);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro detalhado: {ex}");
        throw new InvalidOperationException($"Erro ao carregar o provedor para {messagingType}: {ex.Message}", ex);
    }
}

//public class RebusSubscriptions : IHostedService
//{
//    private readonly IBus _bus;

//    public RebusSubscriptions(IBus bus)
//    {
//        _bus = bus;
//    }

//    public async Task StartAsync(CancellationToken cancellationToken)
//    {
//        await _bus.Subscribe<SaleCreatedEvent>();
//        await _bus.Subscribe<StockConfirmedEvent>();
//        await _bus.Subscribe<StockInsufficientEvent>();
//        await _bus.Subscribe<PaymentConfirmedEvent>();
//        await _bus.Subscribe<PaymentFailEvent>();
//    }

//    public Task StopAsync(CancellationToken cancellationToken)
//    {
//        return Task.CompletedTask;
//    }
//}
