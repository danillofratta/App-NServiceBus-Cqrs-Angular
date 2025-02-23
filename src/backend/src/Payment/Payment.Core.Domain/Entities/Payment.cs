using Base.Core.Domain.Common;
using Payment.Core.Domain.Enum;

namespace PaymentCoreDomainEntities
{
    public class Payment : BaseEntity
    {
        public Guid PaymentRequestId { get; set; }
        public Guid SaleId { get; set; }
        public decimal Total { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Create;
    }
}

