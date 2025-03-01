using Sale.Core.Domain.Contracts.Base;
namespace Sale.Core.Domain.Contracts.Event;
public class ProcessPaymentEvent : IEvent
{
    public Guid SaleId { get; set; }
    public decimal Valor { get; set; }
    public Guid PaymentRequestId { get; set; }
}

