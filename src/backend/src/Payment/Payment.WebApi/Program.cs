using Base.Infrastructure.Messaging;
using Payment.Core.Domain.Application;
using Payment.Core.Domain.Repository;
using Payment.Infrastructure.Orm.Notification;
using Payment.Infrastructure.Orm.Repository;
using Shared.Infrastructure.Orm;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.AddSingleton<NotificationPaymentHubEvent>();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

//builder.Services.AddNServiceBus();
//builder.Services.AddRebusInfrastructure();

string messagingType = builder.Configuration["Messaging:Type"] ?? "Rebus";
ConfigureMessageBus(builder.Services, messagingType);

builder.Services.AddTransient<IMessageSession>(provider =>
{
    var endpointInstance = provider.GetRequiredService<IEndpointInstance>();
    return endpointInstance;
});




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


app.UseRouting();
app.UseAuthorization();

app.UseCors(cors => cors
.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin()
);

app.UseCors((g) => g.AllowAnyOrigin());

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapHub<NotificationPaymentHubEvent>("/NotificationPaymentHub");
});


app.MapGet("/", () => "Payment API está rodando!");


app.Run();

static void ConfigureMessageBus(IServiceCollection services, string messagingType)
{
    string providerNamespace = $"Payment.Infrastructure.{messagingType}";
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
