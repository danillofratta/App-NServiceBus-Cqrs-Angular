using Base.Infrastructure.Messaging;
using Rebus.Bus;
using Rebus.Handlers;

namespace Base.Infrastruture.Messaging.Rebus;

public class RebusAdapter : IMessageBus
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public RebusAdapter(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        return _bus.Publish(message);
    }

    public Task SendAsync<T>(T message, string destination = null, CancellationToken cancellationToken = default) where T : class
    {
        return _bus.Send(message);
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Subscribe<T, THandler>() where T : class where THandler : IMessageHandler<T>
    {

    }

    public void Subscribe<T>()
    {
        _bus.Subscribe<T>().Wait();
    }    
}
