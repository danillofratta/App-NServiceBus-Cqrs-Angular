using Sale.Core.Application.Sales.Create;
using Sale.Core.Domain.Application;
using Sale.Core.Domain.Repository;
using Sale.Infrasctructure.Services.Bus;
using Sale.Infrastructure.Orm.Repository;
using Serilog;
using Shared.Infrastructure;

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

//aqui com servicebus rodando dentro webpi
builder.Services.AddNServiceBus();

//aqui com servicebus rorando em outro serviço
//configura servicebus que é chamado em outro processo fora da webapi
//builder.Services.AddSingleton(provider =>
//{
//    var endpointConfiguration = new EndpointConfiguration("SaleSagaEndpoint");

//    // Defina o transporte correto (exemplo: RabbitMQ, Azure Service Bus, etc.)
//    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
//    transport.UseConventionalRoutingTopology(QueueType.Classic);
//    transport.ConnectionString("amqp://guest:guest@localhost:5672/");// Troque para o transporte real, se necessário
//    endpointConfiguration.UsePersistence<LearningPersistence>();

//    endpointConfiguration.UseSerialization<SystemJsonSerializer>();

//    var endpointInstance = NServiceBus.Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
//    return endpointInstance;
//});

builder.Services.AddScoped<IMessageSession>(provider =>
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
