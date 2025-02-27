using Microsoft.EntityFrameworkCore;
using SaleCoreDomainEntities;
using System.Reflection;

namespace Shared.Infrastructure.Orm;

public class DefaultDbContext : DbContext
{
    public DbSet<SaleCoreDomainEntities.Sale> Sales { get; set; }
    public DbSet<SaleCoreDomainEntities.SaleItens> SaleItens { get; set; }
    public DbSet<StockCoreDomainEntitties.Stock> Stocks { get; set; }
    public DbSet<Product.Core.Domain.Entities.Product> Products { get; set; }

    public DbSet<PaymentCoreDomainEntities.Payment> Payments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {

#if DEBUG
            //run docker with VS
            //todo craete appsettings
            //var conn = "Host=localhost;Port=5432;Username=admin;Password=root;Database=apisalestock;";
            var conn = "Host=localhost;Port=5432;Username=admin;Password=root;Database=apitest;";
            optionsBuilder.UseNpgsql(conn);
#else
        //run docker 
        //todo craete appsettings
        var conn = "Host=postgres_db;Port=5432;Username=admin;Password=root;Database=apitest;";
        optionsBuilder.UseNpgsql(conn);
#endif

        }

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SaleCoreDomainEntities.Sale>()
            .HasMany(s => s.SaleItens)
            .WithOne(si => si.Sale)
            .HasForeignKey(si => si.SaleId);

        modelBuilder.Entity<SaleCoreDomainEntities.Sale>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<StockCoreDomainEntitties.Stock>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Product.Core.Domain.Entities.Product>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<PaymentCoreDomainEntities.Payment>()
            .HasKey(x => x.Id);
    }
}
