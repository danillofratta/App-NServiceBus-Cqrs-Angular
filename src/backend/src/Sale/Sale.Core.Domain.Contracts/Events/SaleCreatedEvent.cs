using Sale.Core.Domain.Contracts.Base;
using Sale.Core.Domain.Contracts.Dto.Entities;

namespace Sale.Core.Domain.Contracts.Event;
public class SaleCreatedEvent : IEvent
{
    public Guid SaleId { get; set; }
    public List<SaleItensDto> SaleItens { get; set; } = new();

    //use in sagamasstransit
    public Guid CorrelationId => SaleId;
}
