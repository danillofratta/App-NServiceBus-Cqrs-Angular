using Automatonymous;
using MassTransit;


namespace Sale.Core.Domain.SagaMassTransit;

public class SaleSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; } // This will be the primary key for the saga
    public string CurrentState { get; set; }

    public Guid SaleId { get; set; }
    public DateTime? ProcessStarted { get; set; } = null;
    public DateTime? StockConfirmed { get; set; } = null;
    public DateTime? PaymentRequested { get; set; } = null;
    public DateTime? StockInsufficient { get; set; } = null;
    public DateTime? PaymentConfirmed { get; set; } = null;
    public DateTime? PaymentCancelled { get; set; } = null;

    // For optimistic concurrency
    public uint RowVersion { get; set; }
}
