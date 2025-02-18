using Base.Core.Domain.Common;

namespace Product.Core.Domain.Entities;

public class Product : BaseEntity
{    
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}