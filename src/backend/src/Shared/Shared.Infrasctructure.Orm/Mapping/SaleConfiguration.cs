using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace Shared.Infrasctructure.Orm.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<SaleCoreDomainEntities.Sale>
{
    public void Configure(EntityTypeBuilder<SaleCoreDomainEntities.Sale> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.BranchId).IsRequired();
        builder.Property(u => u.BranchName).IsRequired().HasMaxLength(200);

        builder.Property(u => u.CustomerId).IsRequired();
        builder.Property(u => u.CustomerName).IsRequired().HasMaxLength(200);

        builder.Property(u => u.SaleItens).IsRequired();
        builder.Property(u => u.TotalAmount).IsRequired();

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(20);  

        builder.HasMany(s => s.SaleItens)
            .WithOne(si => si.Sale)
            .HasForeignKey(si => si.SaleId);
    }
}

