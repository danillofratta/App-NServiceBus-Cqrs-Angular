using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Core.Domain.Saga.Saga.Events
{
    public class ProcessPaymentEvent : IEvent
    {
        public Guid SaleId { get; set; }
        public decimal Valor { get; set; }
        public Guid PaymentRequestId { get; set; }
    }
}
