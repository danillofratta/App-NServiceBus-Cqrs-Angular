using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Payment.Core.Domain.Application.Payment.Service;

namespace Payment.Infrastructure.Orm.Notification
{
    public class NotificationPaymentHub : Hub
    {
        private readonly ILogger<NotificationPaymentHub> _logger;
        private readonly SignalRPaymentNotificationService _notificationService;

        public NotificationPaymentHub(SignalRPaymentNotificationService signalRPaymentNotification, ILogger<NotificationPaymentHub> logger)
        {
            _logger = logger;
            _notificationService = signalRPaymentNotification;
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("GetListPayment", message);
            
            await Clients.Groups("SignalR Users").SendAsync("GetListPayment", message);
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Cliente conectado: {Context.ConnectionId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");            

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Cliente desconectado: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendTestMessage()
        {
            Console.WriteLine("🔹 Teste de evento SignalR enviado!");
            await Clients.All.SendAsync("GetListPayment", new { message = "Teste de conexão" });
        }
    }
}
