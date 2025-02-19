using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Payment.Core.Domain.Enum;
using Payment.Core.Domain.Repository;
using Payment.Core.Domain.Services;
using Sale.Core.Domain.Saga.Commands;
using Sale.Core.Domain.Saga.Events;

namespace Payment.Core.Domain.Application.Payment.Handlers
{

    public class ProcessPaymentHandler : IHandleMessages<ProcessPaymentCommand>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentCreateService _PaymentCreateService;
        public ProcessPaymentHandler(PaymentCreateService paymentCreateService, IPaymentRepository paymentRepository)
        {
            _PaymentCreateService = paymentCreateService;
            _paymentRepository = paymentRepository;
        }

        public async Task Handle(ProcessPaymentCommand message, IMessageHandlerContext context)
        {
            //if existe not create a nem payment
            //var existingPayment = await _paymentRepository.GetByIdSaleAsync(message.SaleId);
            //if (existingPayment == null)
            //{
                var payment = await _PaymentCreateService.Process(message.SaleId, message.Valor);
                if (payment != null)
                {
                    Random random = new Random();
                    bool randomBool = random.Next(2) == 0;

                    if (randomBool)
                    {
                        //todo create class service                    
                        payment.Status = PaymentStatus.Sucefull;
                        await _paymentRepository.UpdateAsync(payment);

                        await context.Publish(new PaymentConfirmedEvent { SaleId = message.SaleId });
                    }
                    else
                    {
                        //todo create class service                    
                        payment.Status = PaymentStatus.Cancelled;
                        await _paymentRepository.UpdateAsync(payment);

                        await context.Publish(new PaymentFailEvent { SaleId = message.SaleId });
                    }
                }
            //}
        }
    }

}
