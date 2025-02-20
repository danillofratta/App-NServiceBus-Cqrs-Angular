using Serilog;
using Shared.Infrastructure;
using Stock.Core.Application;
using Stock.Core.Domain.Repository;
using Stock.Core.Domain.Services;
using Stock.Infrasctructure.Services.Bus;
using Stock.Infrastructure.Orm.Repository;

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

builder.Services.AddNServiceBus();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapGet("/", () => "Stock API está rodando!");

app.Run();
