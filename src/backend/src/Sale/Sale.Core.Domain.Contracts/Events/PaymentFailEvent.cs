namespace Sale.Core.Domain.Contracts.Event;
public class PaymentFailEvent : IEvent
{
    public Guid SaleId { get; set; }
}

