using MassTransit;
using Rebus.Messages;
using Sale.Core.Domain.Contracts.Command;
using Sale.Core.Domain.Service;

namespace Sale.Core.Domain.Application.Sale.Handle
{
    public class SalePaymentCancelledHandle : 
        Rebus.Handlers.IHandleMessages<SalePaymentCancelledCommand>, 
        IHandleMessages<SalePaymentCancelledCommand>,
        IConsumer<SalePaymentCancelledCommand>
    {
        private readonly SalePaymentFailedService _service;

        public SalePaymentCancelledHandle(SalePaymentFailedService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<SalePaymentCancelledCommand> context)
        {
            await _service.Process(context.Message.SaleId);
        }

        public async Task Handle(SalePaymentCancelledCommand message, IMessageHandlerContext context)
        {
            await _service.Process(message.SaleId);
        }

        public async Task Handle(SalePaymentCancelledCommand message)
        {
            await _service.Process(message.SaleId);
        }
    }
}
