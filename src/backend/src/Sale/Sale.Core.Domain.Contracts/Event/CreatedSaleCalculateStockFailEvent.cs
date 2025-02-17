
using Base.Common.Validation;

namespace Sale.Core.Domain.Contracts.Event
{
    public class CreatedSaleCalculateStockFailEvent : IEvent
    {
        public Guid SaleId { get; set; }
        public List<ValidationErrorDetail> Errors { get; set; } = null;
    }
}
