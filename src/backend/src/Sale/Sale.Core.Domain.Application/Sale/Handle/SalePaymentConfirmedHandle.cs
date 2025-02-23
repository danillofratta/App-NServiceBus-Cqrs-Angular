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
    public class SalePaymentConfirmedHandle : IHandleMessages<SalePaymentConfirmedCommand>
    {
        private readonly SalePaymentConfirmedService _service;

        public SalePaymentConfirmedHandle(SalePaymentConfirmedService service)
        {
            _service = service;
        }

        public async Task Handle(SalePaymentConfirmedCommand message, IMessageHandlerContext context)
        {
            await _service.Process(message.SaleId);
        }
    }
}
