using AutoMapper;
using MediatR;
using Sale.Core.Application.Sales.Create;
using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Saga.Commands;
using Sale.Core.Domain.Saga.Events;
using Sale.Core.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Core.Domain.Application.Sale.Handle
{
    public class SaleStockConfirmedHandle : IHandleMessages<SaleStockConfirmedCommand>
    {
        private readonly SaleStockConfirmedService _saleStockConfirmedService;

        public SaleStockConfirmedHandle(SaleStockConfirmedService saleStockConfirmedService)
        {
            _saleStockConfirmedService = saleStockConfirmedService;
        }

        public async Task Handle(SaleStockConfirmedCommand message, IMessageHandlerContext context)
        {
            await _saleStockConfirmedService.Process(message.SaleId);
        }
    }
}
