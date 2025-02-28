using Base.Infrastructure.Messaging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

public class MassTransitAdapter : IMessageBus
{
    private readonly IServiceProvider _serviceProvider;

    public MassTransitAdapter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            await publishEndpoint.Publish(message, cancellationToken);
        }
    }

    public async Task SendAsync<T>(T message, string destination = null, CancellationToken cancellationToken = default) where T : class
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var sendEndpointProvider = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();
            if (string.IsNullOrEmpty(destination))
            {
                var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                await publishEndpoint.Publish(message, cancellationToken);
            }
            else
            {
                var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{destination}"));
                await sendEndpoint.Send(message, cancellationToken);
            }
        }
    }

    public Task StartAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task StopAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public void Subscribe<T, THandler>() where T : class where THandler : IMessageHandler<T> { }
    public void Subscribe<T>() { }
}