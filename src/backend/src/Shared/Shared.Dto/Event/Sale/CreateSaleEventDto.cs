using NServiceBus;
using MediatR;
using Shared.Dto.Command.Sale;

namespace Shared.Dto.Event.Sale;

/// <summary>
/// TODO DTO SHARED? because need the same class
/// </summary>
public class CreateSaleEventDto : IEvent, INotification
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string CustomerId { get; set; }
    public string BranchId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<CreateSaleItemDto> SaleItens { get; set; } = new();
}
