using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Payment.Core.Domain.Repository;
using Payment.Infrastructure.Orm.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Core.Domain.Application.Payment.Service
{
    public class SignalRPaymentNotificationService
    {
        private readonly IHubContext<NotificationPaymentHub> _hubContext;
        //private readonly IServiceProvider _serviceProvider;

        //public SignalRPaymentNotificationService(IHubContext<NotificationPaymentHub> hubContext, IServiceProvider serviceProvider)
        //{
        //    _hubContext = hubContext;
        //    _serviceProvider = serviceProvider;
        //}

        public SignalRPaymentNotificationService(IHubContext<NotificationPaymentHub> hubContext)
        {
            _hubContext = hubContext;           
        }

        public async Task NotifyPaymentsUpdated(List<PaymentCoreDomainEntities.Payment> payments)
        {
            //using (var scope = _serviceProvider.CreateScope()) // Cria um escopo para pegar o repositório
            //{
                //var paymentRepository = scope.ServiceProvider.GetRequiredService<IPaymentRepository>();
                //var payments = await paymentRepository.GetAll();

                //Console.WriteLine($"Enviando lista atualizada para SignalR. Total: {payments.Count}");
                //await _hubContext.Clients.All.SendAsync("GetListPayment", payments);
                await _hubContext.Clients.All.SendAsync("GetListPayment", payments);
            //}
        }
    }
}
