
using Base.Core.Domain.Common;
using Shared.Dto.Enum;

namespace Shared.Dto.Entities;

public class SaleItensDto : BaseEntity
{
    public Guid SaleId { get; set; }

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
    public SaleItemStatusDto Status { get; set; } = SaleItemStatusDto.Create;
}


