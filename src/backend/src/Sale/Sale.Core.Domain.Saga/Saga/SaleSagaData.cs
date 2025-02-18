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
        public bool IsStockConfirmed { get; set; }
        public bool IsPaymentConfirmed { get; set; }
    }
}
