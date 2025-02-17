
namespace Sale.Core.Domain.Contracts.Event
{
    public class CreatedSaleCalculateStockOkEvent : IEvent
    {
        public Guid SaleId { get; set; }
    }
}
