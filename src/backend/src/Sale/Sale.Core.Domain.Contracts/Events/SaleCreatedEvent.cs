using Sale.Core.Domain.Contracts.Dto.Entities;

namespace Sale.Core.Domain.Contracts.Event;
public class SaleCreatedEvent : NServiceBus.IEvent
{
    public Guid SaleId { get; set; }
    public List<SaleItensDto> SaleItens { get; set; } = new();
}
