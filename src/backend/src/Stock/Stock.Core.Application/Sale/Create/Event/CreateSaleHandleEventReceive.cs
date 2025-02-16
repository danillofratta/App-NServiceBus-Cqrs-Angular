using Base.WebApi.Common;
using MediatR;
using Base.Common.Validation;
using Shared.Dto.Event.Sale;
using Stock.Core.Domain.Services;
using AutoMapper;
using Shared.Dto.Entities;

namespace Stock.Core.Application.Sale.Create.Event
{
    /// <summary>
    /// receive messasge after create sale and verify if have item in stock
    /// </summary>
    public class CreateSaleHandleEventReceive : IHandleMessages<CreateSaleEventDto>
    {
        private readonly SubtractAndCheckItemInStockService _service;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CreateSaleHandleEventReceive(IMapper mapper, IMediator mediator, SubtractAndCheckItemInStockService service)
        {
            _service = service;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Handle(CreateSaleEventDto message, IMessageHandlerContext context)
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
            
            listError.AddRange(await _service.SubtractItemFromInventory(record.SaleItens));

            var eventMessage = new CreateSaleEventReponseFromStockDto
            {
                SaleId = message.Id,
                Message = listError.Count > 0 ? "Product not found or quantity above stock " : "Check Stock successfully ",
                Success = listError.Count > 0 ? false : true,
                Errors = listError
            };

            //response to sale
            await context.Publish(eventMessage);
        }
    }
}
