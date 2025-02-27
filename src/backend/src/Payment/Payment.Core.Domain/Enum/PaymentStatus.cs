using System;
using System.Linq;

namespace Payment.Core.Domain.Enum
{
    /// <summary>
    /// TODO: check others status like distribuided, delivery, paymentok, paymentfail, ....
    /// </summary>
    public enum PaymentStatus
    {
        Create = 0,
        Waiting,
        Cancelled,
        Sucefull
    }
}
