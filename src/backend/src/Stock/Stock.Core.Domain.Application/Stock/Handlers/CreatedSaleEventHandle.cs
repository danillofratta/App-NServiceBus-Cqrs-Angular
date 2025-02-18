using MediatR;
using Base.Common.Validation;
using Stock.Core.Domain.Services;
using AutoMapper;
using Sale.Core.Domain.Contracts.Event;
using Sale.Core.Domain.Contracts.Dto.Entities;

namespace Stock.Core.Domain.Application.Stock.Handlers
{
    /// <summary>
    /// receive messasge after create sale and verify if have item in stock
    /// </summary>
    //public class CreatedSaleEventHandle : IHandleMessages<CreatedSaleEvent>
    //{
    //    private readonly CalculateStockService _CalculateStockService;
    //    private readonly CheckItemInStockService _CheckItemInStockService;
    //    private readonly IMapper _mapper;
    //    private List<ValidationErrorDetail> _Errors = new();

    //    public CreatedSaleEventHandle(IMapper mapper, CalculateStockService service, CheckItemInStockService checkItemInStockService)
    //    {
    //        _CalculateStockService = service;
    //        _mapper = mapper;
    //        _CheckItemInStockService = checkItemInStockService;
    //    }

    //    public async Task Handle(CreatedSaleEvent message, IMessageHandlerContext context)
    //    {
    //        await this.CheckStockFail(message);

    //        var eventMessage = new CreatedSaleCalculateStockFailEvent
    //        {
    //            SaleId = message.Id,
    //            Errors = _Errors
    //        };

    //        await context.Publish(eventMessage);
    //    }

    //    private async Task CheckStockFail(CreatedSaleEvent message)
    //    {
    //        //check if item has in stock
    //        foreach (var item in message.SaleItens)
    //        {
    //            await this._CheckItemInStockService.Process(item.ProductId, item.Quantity);
    //            this._Errors = _CheckItemInStockService._ListError;
    //        }

    //        //if not in stcok
    //        if (this._Errors.Count > 0) return;

    //        //if has in stock
    //        SaleDto record = new SaleDto { Id = message.Id };
    //        foreach (var item in message.SaleItens)
    //        {
    //            await _CalculateStockService.Process(item.ProductId, item.Quantity);
    //        }
    //    }
    //}
}
