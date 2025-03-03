using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace Shared.Infrasctructure.Orm.Mapping;

public class StockConfiguration : IEntityTypeConfiguration<StockCoreDomainEntitties.Stock>
{
    public void Configure(EntityTypeBuilder<StockCoreDomainEntitties.Stock> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Quantity).IsRequired();
        builder.Property(u => u.Price).IsRequired();                

        builder.Property(u => u.ProductId).IsRequired();
        builder.Property(u => u.ProductName).IsRequired().HasMaxLength(200);
    }
}

