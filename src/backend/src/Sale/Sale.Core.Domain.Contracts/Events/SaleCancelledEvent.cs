using Sale.Core.Domain.Contracts.Base;
namespace Sale.Core.Domain.Contracts.Event;
public class SaleCancelledEvent : IEvent
{
    public Guid SaleId { get; set; }
}

