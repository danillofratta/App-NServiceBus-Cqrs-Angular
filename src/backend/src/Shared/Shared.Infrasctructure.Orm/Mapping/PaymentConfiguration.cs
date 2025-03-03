using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using SaleCoreDomainEntities;

namespace Shared.Infrasctructure.Orm.Mapping;

public class PaymentConfiguration : IEntityTypeConfiguration<PaymentCoreDomainEntities.Payment>
{
    public void Configure(EntityTypeBuilder<PaymentCoreDomainEntities.Payment> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Total).IsRequired();
        builder.Property(u => u.SaleId).IsRequired();                
    }
}

