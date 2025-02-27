namespace Sale.Core.Domain.Contracts.Event;
public class SaleCancelledEvent : IEvent
{
    public Guid SaleId { get; set; }
}

