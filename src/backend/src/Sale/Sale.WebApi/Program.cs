using Microsoft.AspNetCore.Server.Kestrel;
using NServiceBus;
using NServiceBus.Routing;
using Sale.Core.Application;
using Sale.Core.Application.Sales.Create;
using Sale.Core.Domain.Repository;
using Sale.Infrasctructure.Services.Bus;
using Sale.Infrasctructure.Services.Bus.Config;
using Sale.Infrastructure.Repository;
using Serilog;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services.AddLogging();

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

builder.Services.AddNServiceBus();

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<CreateSaleCommand>();
builder.Services.AddScoped<CreateSaleResult>();
builder.Services.AddScoped<CreateSaleHandler>();
//builder.Services.AddTransient<CreateSaleHandlerEventToStock>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(cors => cors
.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin()
);

app.UseCors((g) => g.AllowAnyOrigin());
app.UseCors((g) => g.AllowCredentials());

app.MapGet("/", () => "NServiceBus est� rodando!");

app.Run();
