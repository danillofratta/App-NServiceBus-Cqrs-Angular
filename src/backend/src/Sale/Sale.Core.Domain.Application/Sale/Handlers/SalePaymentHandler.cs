using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Saga.Events;
using Sale.Core.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Core.Domain.Application.Sale.Handlers
{
    public class SalePaymentHandler : IHandleMessages<PaymentFailEvent>, IHandleMessages<PaymentConfirmedEvent>
    {
        private readonly SalePaymentFailedService _SalePaymentFailedService;
        private readonly SalePaymentConfirmedService _SalePaymentConfirmedService;

        public SalePaymentHandler(SalePaymentFailedService SalePaymentFailedService, SalePaymentConfirmedService SalePaymentConfirmedService)
        {
            _SalePaymentFailedService = SalePaymentFailedService;
            _SalePaymentConfirmedService = SalePaymentConfirmedService;
        }

        public async Task Handle(PaymentFailEvent message, IMessageHandlerContext context)
        {
            await _SalePaymentFailedService.Process(message.SaleId);
        }

        public async Task Handle(PaymentConfirmedEvent message, IMessageHandlerContext context)
        {
            await _SalePaymentConfirmedService.Process(message.SaleId);
        }
    }
}
