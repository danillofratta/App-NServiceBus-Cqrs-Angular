using Base.Common.Validation;
using Sale.Core.Domain.Contracts.Dto.Entities;
using Sale.Core.Domain.Contracts.Event;
using Sale.Core.Domain.Saga.Commands;
using Sale.Core.Domain.Saga.Events;
using Stock.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Domain.Application.Stock.Handlers
{
    public class ReserveStockCommandHandle : IHandleMessages<ReserveStockCommand>
    {
        private List<ValidationErrorDetail> _Errors = new();
        private readonly CalculateStockService _CalculateStockService;
        private readonly CheckItemInStockService _CheckItemInStockService;

        public ReserveStockCommandHandle(CalculateStockService service, CheckItemInStockService CheckItemInStockService)
        {
            _CalculateStockService = service;
            _CheckItemInStockService = CheckItemInStockService;
        }

        public async Task Handle(ReserveStockCommand message, IMessageHandlerContext context)
        {
            await this.CheckStockFail(message);

            if (this._Errors.Count > 0)
            {
                await context.Publish(new StockInsufficientEvent { SaleId = message.SaleId, Errors = _Errors });
            }
            else
            {
                await context.Publish(new StockConfirmedEvent { SaleId = message.SaleId, Total = message.SaleItens.Sum(x => x.TotalPrice) });
            }
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
    }

}
