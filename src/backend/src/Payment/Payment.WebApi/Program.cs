using Payment.Core.Domain.Repository;
using Payment.Infrasctructure;
using Payment.Infrasctructure.Services.Bus;
using Payment.Infrastructure.Orm.Repository;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        //typeof(ApplicationLayer).Assembly,
        typeof(Program).Assembly
    );
});

//builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

builder.Services.AddDbContext<DefaultDbContext>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddNServiceBus();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(cors => cors
.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin()
);

app.UseCors((g) => g.AllowAnyOrigin());
app.UseCors((g) => g.AllowCredentials());

app.MapControllers();

app.MapGet("/", () => "Stock API está rodando!");


app.Run();
