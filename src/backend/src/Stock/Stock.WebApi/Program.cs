
using Base.Infrastructure.Messaging;
using Serilog;
using Shared.Infrastructure.Orm;
using Stock.Core.Application;
using Stock.Core.Domain.Application.Stock.Handlers;
using Stock.Core.Domain.Repository;
using Stock.Core.Domain.Services;
using Stock.Infrastructure.Orm.Repository;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services.AddLogging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
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
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<CalculateStockService>();
builder.Services.AddScoped<CheckItemInStockService>();
builder.Services.AddScoped<ReserveStockCommandHandle>();


//builder.Services.AddNServiceBus();
//builder.Services.AddRebusInfrastructure();
// Configura��o din�mica do IMessageBus via provedores
string messagingType = builder.Configuration["Messaging:Type"] ?? "Rebus";
ConfigureMessageBus(builder.Services, messagingType);


#if DEBUG

#else
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // Porta do container
});
#endif

var app = builder.Build();

// No Program.cs ou Startup.cs ap�s configurar o Rebus
//app.ApplicationServices.UseRebus();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.UseCors(cors => cors
.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin()
);

app.UseCors((g) => g.AllowAnyOrigin());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapGet("/", () => "Stock API est� rodando!");

app.Run();

static void ConfigureMessageBus(IServiceCollection services, string messagingType)
{
    string providerNamespace = $"Stock.Infrastructure.{messagingType}";
    string providerFullName = $"{providerNamespace}.{messagingType}Provider";

    try
    {
        Console.WriteLine($"Tentando carregar assembly: {providerNamespace}");
        var assembly = Assembly.Load($"{providerNamespace}");
        Console.WriteLine($"Assembly carregado com sucesso: {assembly.FullName}");

        var providerType = assembly.GetType(providerFullName);
        if (providerType == null)
        {
            Console.WriteLine($"Tipo {providerFullName} n�o encontrado no assembly.");
            throw new InvalidOperationException($"Provedor para {messagingType} n�o encontrado ou inv�lido.");
        }
        if (!typeof(IMessageBusProvider).IsAssignableFrom(providerType))
        {
            Console.WriteLine($"Tipo {providerFullName} n�o implementa IMessageBusProvider.");
            throw new InvalidOperationException($"Provedor para {messagingType} n�o encontrado ou inv�lido.");
        }

        Console.WriteLine($"Criando inst�ncia de {providerFullName}");
        var provider = Activator.CreateInstance(providerType) as IMessageBusProvider;
        if (provider == null)
        {
            throw new InvalidOperationException($"N�o foi poss�vel criar inst�ncia do provedor para {messagingType}.");
        }
        provider?.Configure(services, messagingType);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro detalhado: {ex}");
        throw new InvalidOperationException($"Erro ao carregar o provedor para {messagingType}: {ex.Message}", ex);
    }
}
