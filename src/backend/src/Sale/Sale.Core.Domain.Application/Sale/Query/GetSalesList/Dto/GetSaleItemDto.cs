using Sale.Core.Domain.Enum;

namespace Sale.Core.Domain.Application.Sale.Query.GetSalesList.Dto;

public class GetSaleItemDto
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
    public SaleItemStatus Status { get; set; } = SaleItemStatus.Create;
}
