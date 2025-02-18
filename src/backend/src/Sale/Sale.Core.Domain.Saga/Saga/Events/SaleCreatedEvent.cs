using Sale.Core.Domain.Contracts.Dto.Entities;

namespace Sale.Core.Domain.Saga.Events
{
    public class SaleCreatedEvent : IEvent
    {
        public Guid SaleId { get; set; }
        public List<SaleItensDto> SaleItens { get; set; }
    }
}