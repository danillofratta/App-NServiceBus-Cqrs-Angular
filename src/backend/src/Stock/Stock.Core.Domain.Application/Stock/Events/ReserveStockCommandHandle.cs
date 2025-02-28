using Base.Common.Validation;
using Sale.Core.Domain.Contracts.Command;
using Sale.Core.Domain.Contracts.Dto.Entities;
using Sale.Core.Domain.Contracts.Event;
using Stock.Core.Domain.Services;
using Base.Infrastructure.Messaging;
using MassTransit;
using Rebus.Messages;

namespace Stock.Core.Domain.Application.Stock.Handlers
{
    public class ReserveStockCommandHandle : 
        Rebus.Handlers.IHandleMessages<ReserveStockCommand>, 
        IHandleMessages<ReserveStockCommand>,
        IConsumer<ReserveStockCommand>
    {
        private List<ValidationErrorDetail> _Errors = new();
        private readonly CalculateStockService _CalculateStockService;
        private readonly CheckItemInStockService _CheckItemInStockService;
        private readonly IMessageBus _bus;

        public ReserveStockCommandHandle(IMessageBus bus, CalculateStockService service, CheckItemInStockService CheckItemInStockService)
        {
            _CalculateStockService = service;
            _CheckItemInStockService = CheckItemInStockService;
            _bus = bus;
        }

        private async Task CheckStockFail(ReserveStockCommand message)
        {
            //check if item has in stock
            foreach (var item in message.SaleItens)
            {
                await this._CheckItemInStockService.Process(item.ProductId, item.Quantity);
                this._Errors = _CheckItemInStockService._ListError;
            }

            //if not in stcok
            if (this._Errors.Count > 0) return;

            //if has in stock
            SaleDto record = new SaleDto { Id = message.SaleId };
            foreach (var item in message.SaleItens)
            {
                await _CalculateStockService.Process(item.ProductId, item.Quantity);
            }
        }

        async Task Rebus.Handlers.IHandleMessages<ReserveStockCommand>.Handle(ReserveStockCommand message)
        {
            await this.CheckStockFail(message);

            if (this._Errors.Count > 0)
            {
                await _bus.PublishAsync(new StockInsufficientEvent { SaleId = message.SaleId, Errors = _Errors });
            }
            else
            {
                await _bus.PublishAsync(new StockConfirmedEvent { SaleId = message.SaleId, Total = message.SaleItens.Sum(x => x.TotalPrice) });
            }
        }

        public async Task Handle(ReserveStockCommand message, IMessageHandlerContext context)
        {
            await this.CheckStockFail(message);

            if (this._Errors.Count > 0)
            {
                await _bus.PublishAsync(new StockInsufficientEvent { SaleId = message.SaleId, Errors = _Errors });
            }
            else
            {
                await _bus.PublishAsync(new StockConfirmedEvent { SaleId = message.SaleId, Total = message.SaleItens.Sum(x => x.TotalPrice) });
            }
        }

        public async Task Consume(ConsumeContext<ReserveStockCommand> context)
        {
            await this.CheckStockFail(context.Message);

            if (this._Errors.Count > 0)
            {
                await _bus.PublishAsync(new StockInsufficientEvent { SaleId = context.Message.SaleId, Errors = _Errors });
            }
            else
            {
                await _bus.PublishAsync(new StockConfirmedEvent { SaleId = context.Message.SaleId, Total = context.Message.SaleItens.Sum(x => x.TotalPrice) });
            }
        }
    }    
}
