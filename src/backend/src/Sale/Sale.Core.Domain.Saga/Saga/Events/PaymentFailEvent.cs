namespace Sale.Core.Domain.Saga.Events
{
    public class PaymentFailEvent : IEvent
    {
        public Guid SaleId { get; set; }
    }
}
