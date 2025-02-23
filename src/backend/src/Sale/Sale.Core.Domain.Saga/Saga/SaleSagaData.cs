using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Core.Domain.Saga
{
    public class SaleSagaData : ContainSagaData
    {
        public Guid SaleId { get; set; }
        public DateTime? ProcessStarted { get; set; } = null;
        public DateTime? StockConfirmed { get; set; } = null;
        public DateTime? PaymentRequested { get; set; } = null; 
        public DateTime? StockInsufficient { get; set; } = null;
        public DateTime? PaymentConfirmed { get; set; } = null;
        public DateTime? PaymentCancelled { get; set; } = null;

        public Guid? PaymentRequestId { get; set; } = null;
    }
}
