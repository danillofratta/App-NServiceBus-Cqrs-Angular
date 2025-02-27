namespace Base.Core.Domain.Messaging;

public interface ICommand
{
    Guid Id { get; }
    DateTime Timestamp { get; }
}

