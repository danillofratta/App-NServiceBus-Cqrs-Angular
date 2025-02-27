namespace Base.Core.Domain.Messaging;

public interface IEvent
{
    Guid Id { get; }
    DateTime Timestamp { get; }
}