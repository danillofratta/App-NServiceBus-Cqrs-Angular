
namespace Base.Infrastructure.Messaging.NServiceBus;

public class NServiceBusAdapter : IMessageBus
{
    private readonly IMessageSession _session;

    public NServiceBusAdapter(IMessageSession session)
    {
        _session = session;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        return _session.Publish(message, cancellationToken);
    }


    public Task SendAsync<T>(T message, string destination = null, CancellationToken cancellationToken = default) where T : class
    {
        if (destination == null)
        {
            return _session.Send(message, cancellationToken);
        }
        return _session.Send(destination, message, cancellationToken);
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
        
    }
}
