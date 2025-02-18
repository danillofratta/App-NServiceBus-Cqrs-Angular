namespace Sale.Core.Domain.Saga.Events
{
    public class SaleCancelledEvent : IEvent
    {
        public Guid SaleId { get; set; }
    }
}
