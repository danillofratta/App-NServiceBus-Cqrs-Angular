using Microsoft.AspNetCore.SignalR;
using Sale.Core.Domain.Saga.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Orm.Notification
{
    public class NotificationPaymentHubEvent : Hub<IEmitPayments>
    {
    }

    public interface IEmitPayments
    {
        //Task GetListPayment(ProcessPaymentCommand command);
        Task GetListPayment(List<PaymentCoreDomainEntities.Payment> list);
    }
}
