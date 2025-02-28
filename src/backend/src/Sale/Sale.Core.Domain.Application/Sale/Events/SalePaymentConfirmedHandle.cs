using MassTransit;
using Rebus.Messages;
using Sale.Core.Domain.Contracts.Command;
using Sale.Core.Domain.Service;

namespace Sale.Core.Domain.Application.Sale.Handle
{
    public class SalePaymentConfirmedHandle : 
        Rebus.Handlers.IHandleMessages<SalePaymentConfirmedCommand>,
        IHandleMessages<SalePaymentConfirmedCommand>,
        IConsumer<SalePaymentConfirmedCommand>
    {
        private readonly SalePaymentConfirmedService _service;

        public SalePaymentConfirmedHandle(SalePaymentConfirmedService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<SalePaymentConfirmedCommand> context)
        {
            await _service.Process(context.Message.SaleId);
        }

        public async Task Handle(SalePaymentConfirmedCommand message, IMessageHandlerContext context)
        {
            await _service.Process(message.SaleId);
        }

        public async Task Handle(SalePaymentConfirmedCommand message)
        {
            await _service.Process(message.SaleId);
        }
    }
}
