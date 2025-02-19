
using Payment.Core.Domain.Enum;

namespace Payment.Core.Domain.Application.Payment.Query.GetSalesList
{
    public class GetListPaymentQueryResult
    {
        public Guid Id { get; set; }
        public string SaleId { get; set; }
        public decimal Total { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
