using Sale.Core.Domain.Contracts.Dto.Entities;

namespace Sale.Core.Domain.Contracts.Event;

/// <summary>
/// TODO DTO SHARED? because need the same class
/// </summary>
public class CreatedSaleEvent : IEvent
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string CustomerId { get; set; }
    public string BranchId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<SaleItensDto> SaleItens { get; set; } = new();
}

