using Base.Common.Validation;
using Base.WebApi.Common;
using MediatR;

namespace Shared.Dto.Event.Sale
{
    /// <summary>
    /// TODO: implement default return class?
    /// </summary>
    public class CreateSaleEventReponseFromStockDto : ApiResponseEvent,   IEvent, INotification
    {
        public Guid SaleId { get; set; }
    }
}
