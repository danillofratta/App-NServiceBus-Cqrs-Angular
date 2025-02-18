namespace Sale.Core.Domain.Saga.Events
{
    public class SaleFinishedEvent : IEvent
    {
        public Guid SaleId { get; set; }
    }
}
