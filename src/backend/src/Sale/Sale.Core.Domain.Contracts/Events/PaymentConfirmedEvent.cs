namespace Sale.Core.Domain.Contracts.Event;
public class PaymentConfirmedEvent : IEvent
{
    public Guid SaleId { get; set; }
}

