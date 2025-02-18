
namespace Sale.Core.Domain.Saga.Events
{
    public class StockConfirmedEvent : IEvent
    {
        public Guid SaleId { get; set; }
        public decimal Total { get; set; }
    }
}
