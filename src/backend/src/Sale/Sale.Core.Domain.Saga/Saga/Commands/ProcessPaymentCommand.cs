namespace Sale.Core.Domain.Saga.Commands
{
    public class ProcessPaymentCommand : ICommand
    {
        public Guid SaleId { get; set; }
        public decimal Valor { get; set; }
    }
}
