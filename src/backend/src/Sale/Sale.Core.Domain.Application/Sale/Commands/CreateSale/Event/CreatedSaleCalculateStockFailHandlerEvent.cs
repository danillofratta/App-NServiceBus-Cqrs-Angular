using Microsoft.Extensions.Logging;
using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Enum;
using Sale.Core.Domain.Contracts.Event;

namespace Sale.Core.Application.Sale.Commands.CreateSale.Event
{
    /// <summary>
    /// Response from stock if have products in stock
    /// </summary>
    public class CreatedSaleCalculateStockFailHandlerEvent : IHandleMessages<CreatedSaleCalculateStockFailEvent>
    {
        private readonly ILogger<CreatedSaleCalculateStockFailHandlerEvent> _logger;
        private readonly ISaleRepository _repository;

        public CreatedSaleCalculateStockFailHandlerEvent(ISaleRepository repository, ILogger<CreatedSaleCalculateStockFailHandlerEvent> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task Handle(CreatedSaleCalculateStockFailEvent message, IMessageHandlerContext context)
        {
            var sale = await _repository.GetByIdAsync(message.SaleId);
            if (sale != null)
            {
                sale.Status = SaleStatus.OutOfStock;
                await _repository.UpdateAsync(sale);
            }

            await Task.CompletedTask;
        }
    }
}
