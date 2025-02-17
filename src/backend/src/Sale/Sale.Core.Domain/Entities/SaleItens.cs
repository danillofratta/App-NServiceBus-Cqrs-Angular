
using Base.Core.Domain.Common;
using Sale.Core.Domain.Enum;

namespace SaleCoreDomainEntities;

public class SaleItens : BaseEntity
{
    public Guid SaleId { get; set; } 
    public Sale Sale { get; set; } = null!;

    /// <summary>
    /// External Identities
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// External Identities
    /// </summary>
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
    public SaleItemStatus Status { get; set; } = SaleItemStatus.Create;
}


