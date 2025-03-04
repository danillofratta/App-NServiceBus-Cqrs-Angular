﻿
using MassTransit;
using Rebus.Messages;
using Sale.Core.Domain.Contracts.Command;
using Sale.Core.Domain.Service;


namespace Sale.Core.Domain.Application.Sale.Handle
{
    public class SaleStockInsufficienHandle : 
        Rebus.Handlers.IHandleMessages<SaleStockInsufficienCommand>, 
        IHandleMessages<SaleStockInsufficienCommand>,
        IConsumer<SaleStockInsufficienCommand>
    {
        private readonly SaleStockInsufficientService _SaleStockInsufficientService;

        public SaleStockInsufficienHandle(SaleStockInsufficientService saleStockInsufficientService)
        {
            _SaleStockInsufficientService = saleStockInsufficientService;
        }

        public async Task Consume(ConsumeContext<SaleStockInsufficienCommand> context)
        {
            await _SaleStockInsufficientService.Process(context.Message.SaleId);
        }

        public async Task Handle(SaleStockInsufficienCommand message, IMessageHandlerContext context)
        {
            await _SaleStockInsufficientService.Process(message.SaleId);
        }

        public async Task Handle(SaleStockInsufficienCommand message)
        {
            await _SaleStockInsufficientService.Process(message.SaleId);
        }
    }
}
