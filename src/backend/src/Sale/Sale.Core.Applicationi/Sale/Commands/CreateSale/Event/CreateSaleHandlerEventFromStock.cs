using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Shared.Dto.Event.Sale;
using MediatR;
using System.Threading;
using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Enum;

namespace Sale.Core.Application.Sale.Commands.CreateSale.Event
{
    /// <summary>
    /// Response from stock if have products in stock
    /// </summary>
    public class CreateSaleHandlerEventFromStock : IHandleMessages<CreateSaleEventReponseFromStockDto>
    {
        private readonly ILogger<CreateSaleHandlerEventToStock> _logger;
        private readonly ISaleRepository _repository;

        public CreateSaleHandlerEventFromStock(ISaleRepository repository, ILogger<CreateSaleHandlerEventToStock> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task Handle(CreateSaleEventReponseFromStockDto message, IMessageHandlerContext context)
        {
            if (!message.Success)
            {
                var sale = await _repository.GetByIdAsync(message.SaleId);
                if (sale != null)
                {
                    sale.Status = SaleStatus.OutOfStock;
                    await _repository.UpdateAsync(sale);
                }
            }

            await Task.CompletedTask;
        }
    }
}
