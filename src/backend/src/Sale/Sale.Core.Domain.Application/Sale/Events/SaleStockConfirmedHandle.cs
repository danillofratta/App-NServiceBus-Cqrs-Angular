using MassTransit;
using Rebus.Messages;
using Sale.Core.Domain.Contracts.Command;
using Sale.Core.Domain.Service;

namespace Sale.Core.Domain.Application.Sale.Handle
{
    public class SaleStockConfirmedHandle : 
        Rebus.Handlers.IHandleMessages<SaleStockConfirmedCommand>, 
        IHandleMessages<SaleStockConfirmedCommand>,
        IConsumer<SaleStockConfirmedCommand>
    {
        private readonly SaleStockConfirmedService _saleStockConfirmedService;

        public SaleStockConfirmedHandle(SaleStockConfirmedService saleStockConfirmedService)
        {
            _saleStockConfirmedService = saleStockConfirmedService;
        }

        public async Task Consume(ConsumeContext<SaleStockConfirmedCommand> context)
        {
            await _saleStockConfirmedService.Process(context.Message.SaleId);
        }

        public async Task Handle(SaleStockConfirmedCommand message, IMessageHandlerContext context)
        {
            await _saleStockConfirmedService.Process(message.SaleId);
        }

        public async Task Handle(SaleStockConfirmedCommand message)
        {
            await _saleStockConfirmedService.Process(message.SaleId);
        }
    }
}
