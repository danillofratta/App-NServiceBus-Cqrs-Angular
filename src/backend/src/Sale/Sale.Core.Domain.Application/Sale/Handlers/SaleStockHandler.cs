using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Saga.Events;
using Sale.Core.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Sale.Core.Domain.Application.Sale.Handlers
//{
//    public class SaleStockHandler : IHandleMessages<StockInsufficientEvent>, IHandleMessages<SaleCancelledEvent>, IHandleMessages<StockConfirmedEvent>
//    {
//        private readonly SaleStockConfirmedService _SaleStockConfirmedService;
//        private readonly SaleStockInsufficientService _SaleStockInsufficientService;

//        public SaleStockHandler(SaleStockConfirmedService SaleStockConfirmedService, SaleStockInsufficientService saleStockInsufficientService)
//        {
//            _SaleStockConfirmedService = SaleStockConfirmedService;
//            _SaleStockInsufficientService = saleStockInsufficientService;
//        }

//        public async Task Handle(StockInsufficientEvent message, IMessageHandlerContext context)
//        {
//            await _SaleStockInsufficientService.Process(message.SaleId);
//        }

//        public async Task Handle(StockConfirmedEvent message, IMessageHandlerContext context)
//        {
//            await _SaleStockConfirmedService.Process(message.SaleId);
//        }

//        public async Task Handle(SaleCancelledEvent message, IMessageHandlerContext context)
//        {
//            await _SaleStockInsufficientService.Process(message.SaleId);
//        }
//    }
//}
