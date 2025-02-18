namespace Sale.Core.Domain.Saga.Events
{
    public class PaymentConfirmedEvent : IEvent
    {
        public Guid SaleId { get; set; }
    }
}
