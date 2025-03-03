using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrasctructure.Orm.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product.Core.Domain.Entities.Product>
{
    public void Configure(EntityTypeBuilder<Product.Core.Domain.Entities.Product> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name).IsRequired().HasMaxLength(200);
        builder.Property(u => u.Price).IsRequired();
    }
}

