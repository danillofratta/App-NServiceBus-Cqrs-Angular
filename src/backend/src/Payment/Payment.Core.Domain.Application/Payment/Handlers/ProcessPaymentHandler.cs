using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Payment.Core.Domain.Enum;
using Payment.Core.Domain.Repository;
using Payment.Core.Domain.Services;
using Sale.Core.Domain.Saga.Commands;
using Sale.Core.Domain.Saga.Events;
using Payment.Infrastructure.Orm.Notification;
using Sale.Core.Domain.Saga.Saga.Events;

namespace Payment.Core.Domain.Application.Payment.Handlers
{

    public class ProcessPaymentHandler : IHandleMessages<ProcessPaymentCommand> //IHandleMessages<ProcessPaymentEvent>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentCreateService _PaymentCreateService;
        readonly IHubContext<NotificationPaymentHubEvent> _hub;

        private readonly IHubContext<NotificationPaymentHubEvent, IEmitPayments> _hub1;
        public ProcessPaymentHandler(IHubContext<
            NotificationPaymentHubEvent> hub,
            IHubContext<NotificationPaymentHubEvent, IEmitPayments> hub1,
            PaymentCreateService paymentCreateService, IPaymentRepository paymentRepository)
        {
            _PaymentCreateService = paymentCreateService;
            _paymentRepository = paymentRepository;
            this._hub = hub;
            this._hub1 = hub1;
        }

        public async Task Handle(ProcessPaymentCommand message, IMessageHandlerContext context)
        {
            var hasprocess = await _paymentRepository.GetByPaymentRequestId(message.PaymentRequestId);

            if (hasprocess == null)
            {
                var haspayment = await _paymentRepository.GetByIdSaleAsync(message.SaleId);                
                //verificar triplicando payments, a principio na criação da saga
                //isso garante que não vai duplicar criação de paymentes
                if (haspayment != null && haspayment.CreatedAt <= DateTime.UtcNow.AddSeconds(10))
                    return;

                var payment = await _PaymentCreateService.Process(message.SaleId, message.Valor);
                if (payment != null)
                {
                    if (payment.Status == PaymentStatus.Sucefull || payment.Status == PaymentStatus.Cancelled)
                        return;

                    Random random = new Random();
                    bool randomBool = random.Next(2) == 0;

                    if (randomBool)
                    {
                        //todo create class service                    
                        payment.Status = PaymentStatus.Sucefull;
                        await _paymentRepository.UpdateAsync(payment);

                        List<PaymentCoreDomainEntities.Payment> listpayment = await _paymentRepository.GetAll();
                        await _hub.Clients.All.SendAsync("GetListPayment", listpayment);

                        //await _hub1.Clients.All.GetListPayment(listpayment);

                        await context.Publish(new PaymentConfirmedEvent { SaleId = message.SaleId });
                    }
                    else
                    {
                        //todo create class service                    
                        payment.Status = PaymentStatus.Cancelled;
                        await _paymentRepository.UpdateAsync(payment);

                        List<PaymentCoreDomainEntities.Payment> listpayment = await _paymentRepository.GetAll();
                        await _hub.Clients.All.SendAsync("GetListPayment", listpayment);

                        //await _hub1.Clients.All.GetListPayment(listpayment);

                        await context.Publish(new PaymentFailEvent { SaleId = message.SaleId });
                    }
                }
            }
        }

        //        public async Task Handle(ProcessPaymentEvent message, IMessageHandlerContext context)
        //        {
        //            var payment = await _PaymentCreateService.Process(message.SaleId, message.Valor);
        //            if (payment != null)
        //            {
        //                if (payment.Status == PaymentStatus.Sucefull || payment.Status == PaymentStatus.Cancelled)
        //                    return;

        //                Random random = new Random();
        //                bool randomBool = random.Next(2) == 0;

        //                if (randomBool)
        //                {
        //                    todo create class service
        //                    payment.Status = PaymentStatus.Sucefull;
        //                    await _paymentRepository.UpdateAsync(payment);

        //        List<PaymentCoreDomainEntities.Payment> listpayment = await _paymentRepository.GetAll();
        //        await _hub.Clients.All.SendAsync("GetListPayment", listpayment);

        //        await _hub1.Clients.All.GetListPayment(listpayment);

        //        await context.Publish(new PaymentConfirmedEvent { SaleId = message.SaleId
        //    });
        //                }
        //                else
        //{
        //    todo create class service
        //                    payment.Status = PaymentStatus.Cancelled;
        //await _paymentRepository.UpdateAsync(payment);

        //List<PaymentCoreDomainEntities.Payment> listpayment = await _paymentRepository.GetAll();
        //await _hub.Clients.All.SendAsync("GetListPayment", listpayment);

        //await _hub1.Clients.All.GetListPayment(listpayment);

        //await context.Publish(new PaymentFailEvent { SaleId = message.SaleId });
        //                }
        //            }
        //        }
    }

}
