using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Core.Domain.Enum
{
    /// <summary>
    /// TODO: check others status like distribuided, delivery, paymentok, paymentfail, ....
    /// </summary>
    public enum SaleItemStatus
    {
        Create =0,
        Cancelled
    }
}
