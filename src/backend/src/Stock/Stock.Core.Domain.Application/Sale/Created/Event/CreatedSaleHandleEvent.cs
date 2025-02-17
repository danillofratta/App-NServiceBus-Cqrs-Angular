using MediatR;
using Base.Common.Validation;
using Stock.Core.Domain.Services;
using AutoMapper;
using Sale.Core.Domain.Contracts.Event;
using Sale.Core.Domain.Contracts.Dto.Entities;

namespace Stock.Core.Application.Sale.Create.Event
{
    /// <summary>
    /// receive messasge after create sale and verify if have item in stock
    /// </summary>
    public class CreatedSaleHandleEvent : IHandleMessages<CreatedSaleEvent>
    {
        private readonly CalculateStockService _service;
        private readonly IMapper _mapper;

        public CreatedSaleHandleEvent(IMapper mapper, CalculateStockService service)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task Handle(CreatedSaleEvent message, IMessageHandlerContext context)
        {
            List<ValidationErrorDetail> listError = new();

            SaleDto record = new SaleDto { Id = message.Id};            
            foreach (var item in message.SaleItens)
            {
                SaleItensDto saleitem = new();
                saleitem.ProductId = item.ProductId;
                saleitem.Quantity = item.Quantity;

                record.SaleItens.Add(saleitem);
            }
            
            listError.AddRange(await _service.Calculate(record.SaleItens));

            //if have error return to sale
            if (listError.Count > 0)
            {
                var eventMessage = new CreatedSaleCalculateStockFailEvent
                {
                    SaleId = message.Id,
                    Errors = listError
                };

                //response to sale
                await context.Publish(eventMessage);
            }
        }
    }
}
