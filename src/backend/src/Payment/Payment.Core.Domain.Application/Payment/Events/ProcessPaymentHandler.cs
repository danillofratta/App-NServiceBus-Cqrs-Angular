using Base.Infrastructure.Messaging;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Payment.Core.Domain.Enum;
using Payment.Core.Domain.Repository;
using Payment.Core.Domain.Services;
using Payment.Infrastructure.Orm.Notification;
using Rebus.Messages;
using Sale.Core.Domain.Contracts.Command;
using Sale.Core.Domain.Contracts.Event;

namespace Payment.Core.Domain.Application.Payment.Handlers
{

    public class ProcessPaymentHandler : 
        Rebus.Handlers.IHandleMessages<ProcessPaymentCommand>, 
        IHandleMessages<ProcessPaymentCommand>,
        IConsumer<ProcessPaymentCommand>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentCreateService _PaymentCreateService;
        readonly IHubContext<NotificationPaymentHubEvent> _hub;
        private readonly IMessageBus _bus;

        public ProcessPaymentHandler(
            IMessageBus bus,
            IHubContext<NotificationPaymentHubEvent> hub,
            PaymentCreateService paymentCreateService, IPaymentRepository paymentRepository)
        {
            _PaymentCreateService = paymentCreateService;
            _paymentRepository = paymentRepository;
            this._hub = hub;
            this._bus = bus;
        }


        //TODO fix duplicate code
        //using mass
        public async Task Consume(ConsumeContext<ProcessPaymentCommand> context)
        {
            var hasprocess = await _paymentRepository.GetByPaymentRequestId(context.Message.PaymentRequestId);

            if (hasprocess == null)
            {
                var haspayment = await _paymentRepository.GetByIdSaleAsync(context.Message.SaleId);
                //verificar triplicando payments, a principio na criação da saga
                //isso garante que não vai duplicar criação de paymentes
                if (haspayment != null && haspayment.CreatedAt <= DateTime.UtcNow.AddSeconds(10))
                    return;

                var payment = await _PaymentCreateService.Process(context.Message.SaleId, context.Message.Valor);
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

                        await _bus.PublishAsync(new PaymentConfirmedEvent { SaleId = context.Message.SaleId });
                    }
                    else
                    {
                        //todo create class service                    
                        payment.Status = PaymentStatus.Cancelled;
                        await _paymentRepository.UpdateAsync(payment);

                        List<PaymentCoreDomainEntities.Payment> listpayment = await _paymentRepository.GetAll();
                        await _hub.Clients.All.SendAsync("GetListPayment", listpayment);

                        await _bus.PublishAsync(new PaymentFailEvent { SaleId = context.Message.SaleId });
                    }
                }
            }
        }

        //using nservicebus
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

                        await _bus.PublishAsync(new PaymentConfirmedEvent { SaleId = message.SaleId });
                    }
                    else
                    {
                        //todo create class service                    
                        payment.Status = PaymentStatus.Cancelled;
                        await _paymentRepository.UpdateAsync(payment);

                        List<PaymentCoreDomainEntities.Payment> listpayment = await _paymentRepository.GetAll();
                        await _hub.Clients.All.SendAsync("GetListPayment", listpayment);

                        await _bus.PublishAsync(new PaymentFailEvent { SaleId = message.SaleId });
                    }
                }
            }
        }

        //TODO fix duplicate code
        //using rebus
        public async Task Handle(ProcessPaymentCommand message)
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

                        await _bus.PublishAsync(new PaymentConfirmedEvent { SaleId = message.SaleId });
                    }
                    else
                    {
                        //todo create class service                    
                        payment.Status = PaymentStatus.Cancelled;
                        await _paymentRepository.UpdateAsync(payment);

                        List<PaymentCoreDomainEntities.Payment> listpayment = await _paymentRepository.GetAll();
                        await _hub.Clients.All.SendAsync("GetListPayment", listpayment);

                        //await _hub1.Clients.All.GetListPayment(listpayment);

                        await _bus.PublishAsync(new PaymentFailEvent { SaleId = message.SaleId });
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
