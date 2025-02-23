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
    public class SalePaymentCanceleldHandle : IHandleMessages<SalePaymentCancelledCommand>
    {
        private readonly SalePaymentFailedService _service;

        public SalePaymentCanceleldHandle(SalePaymentFailedService service)
        {
            _service = service;
        }

        public async Task Handle(SalePaymentCancelledCommand message, IMessageHandlerContext context)
        {
            await _service.Process(message.SaleId);
        }
    }
}
