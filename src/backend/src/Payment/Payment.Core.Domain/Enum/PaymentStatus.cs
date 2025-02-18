using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Core.Domain.Enum
{
    /// <summary>
    /// TODO: check others status like distribuided, delivery, paymentok, paymentfail, ....
    /// </summary>
    public enum PaymentStatus
    {
        Create =0,
        Waiting,
        Cancelled,
        Sucefull
    }
}
