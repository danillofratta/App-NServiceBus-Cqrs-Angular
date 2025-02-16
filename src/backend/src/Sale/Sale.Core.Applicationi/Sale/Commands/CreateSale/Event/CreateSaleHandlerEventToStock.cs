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

namespace Sale.Core.Application.Sale.Commands.CreateSale.Event
{
    /// <summary>
    /// Send sale to stock to check if products are available according to sale quantity
    /// </summary>
    public class CreateSaleHandlerEventToStock : INotificationHandler<CreateSaleEventDto>
    {
        private readonly ILogger<CreateSaleHandlerEventToStock> _logger;
        private readonly IMessageSession _messageSession;

        public CreateSaleHandlerEventToStock(ILogger<CreateSaleHandlerEventToStock> logger, IMessageSession messageSession)
        {
            _logger = logger;
            _messageSession = messageSession;
        }

        public async Task Handle(CreateSaleEventDto notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Sale Created - Id: {Id}, Number: {Number}, Customer: {CustomerId}, Total: {Total}",
                notification.Id,
                notification.Number,
                notification.CustomerId,
                notification.TotalAmount);

            await _messageSession.Publish(notification);

            await Task.CompletedTask;
        }
    }
}
