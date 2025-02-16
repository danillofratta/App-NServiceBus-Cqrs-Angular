using Base.WebApi.Common;
using MediatR;
using Shared.Dto.Event.Sale;

namespace Stock.Core.Application.Sale.Create.Event
{
    public class CreateSaleHandleEventReponse : INotificationHandler<CreateSaleEventReponseFromStockDto>
    {
        private readonly IEndpointInstance _endpointInstance;

        public CreateSaleHandleEventReponse(IEndpointInstance endpointInstance)
        {
            _endpointInstance = endpointInstance;
        }

        public async Task Handle(CreateSaleEventReponseFromStockDto notification, CancellationToken cancellationToken)
        {
            // Usando o endpointInstance para publicar a mensagem
            await _endpointInstance.Publish(notification);
        }
    }
}
