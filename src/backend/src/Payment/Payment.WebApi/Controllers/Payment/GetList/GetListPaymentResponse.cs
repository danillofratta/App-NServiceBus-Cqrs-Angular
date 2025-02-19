
using Payment.Core.Domain.Enum;

namespace Payment.WebApi.Controllers.Payment.GetList
{
    public class GetListPaymentResponse
    {
        public Guid Id { get; set; }
        public string SaleId { get; set; }
        public decimal Total { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
