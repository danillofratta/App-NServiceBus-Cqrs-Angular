using Microsoft.AspNetCore.SignalR;

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
