
namespace Sale.Core.Domain.Contracts.Event;
public class StockConfirmedEvent : IEvent
{
    public Guid SaleId { get; set; }
    public decimal Total { get; set; }
}

