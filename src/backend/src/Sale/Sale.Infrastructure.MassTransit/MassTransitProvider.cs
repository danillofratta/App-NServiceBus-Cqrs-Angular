using Base.Infrastructure.Messaging;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sale.Core.Domain.Application.Sale.Handle;
using Sale.Core.Domain.SagaMassTransit;
using Sale.Core.Domain.Service;

namespace Sale.Infrastructure.MassTransit
{
    public class MassTransitProvider : IMessageBusProvider
    {
        public void Configure(IServiceCollection services, string messagingType)
        {
            if (messagingType.ToLower() != "masstransit") return;

            services.AddScoped<SalePaymentFailedService>();
            services.AddScoped<SalePaymentConfirmedService>();
            services.AddScoped<SaleStockConfirmedService>();
            services.AddScoped<SaleStockInsufficientService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SalePaymentCancelledHandle>();
                x.AddConsumer<SalePaymentConfirmedHandle>();                
                x.AddConsumer<SaleStockConfirmedHandle>();
                x.AddConsumer<SaleStockInsufficienHandle>();

                x.AddSagaStateMachine<SaleSagaStateMachine, SaleSagaState>()
                .InMemoryRepository();

                //TODO
                //.EntityFrameworkRepository(r =>
                //{
                //    r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                //    r.AddDbContext<DbContext, SaleSagaDbContext>((provider, builder) =>
                //    {
                //        builder.UseNpgsql("Host=localhost;Database=apitest;Username=admin;Password=root");
                //    });
                //});
                //
                x.UsingRabbitMq((context, cfg) =>
                {                    
#if DEBUG
                    cfg.Host(new Uri("amqp://guest:guest@localhost:5672/"));
#else
                    cfg.Host(new Uri("amqp://guest:guest@rabbitmq:5672/"));                    
#endif

                    cfg.ReceiveEndpoint("SaleSagaEndpoint", e =>
                    {
                        e.ConfigureSaga<SaleSagaState>(context);
                        e.ConfigureConsumer<SalePaymentCancelledHandle>(context);
                        e.ConfigureConsumer<SalePaymentConfirmedHandle>(context);
                        e.ConfigureConsumer<SaleStockConfirmedHandle>(context);
                        e.ConfigureConsumer<SaleStockInsufficienHandle>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                    cfg.UseRawJsonSerializer();
                });
            });

            services.AddMassTransitHostedService();
            services.AddScoped<IMessageBus, MassTransitAdapter>();
        }
    }

    public class SaleSagaDbContext : DbContext
    {
        public SaleSagaDbContext(DbContextOptions<SaleSagaDbContext> options) : base(options) { }
        public DbSet<SaleSagaState> SaleSagaStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure CorrelationId as the primary key
            modelBuilder.Entity<SaleSagaState>()
                .HasKey(s => s.CorrelationId); // Explicitly set CorrelationId as the primary key

            // Configure RowVersion for optimistic concurrency
            modelBuilder.Entity<SaleSagaState>()
                .Property(s => s.RowVersion)
                .IsConcurrencyToken();

            // Optional: Name the table explicitly
            modelBuilder.Entity<SaleSagaState>().ToTable("SaleSagaState");
        }
    }
}