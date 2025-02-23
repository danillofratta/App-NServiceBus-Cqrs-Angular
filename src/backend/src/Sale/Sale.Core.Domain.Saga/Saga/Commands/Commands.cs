using Sale.Core.Domain.Contracts.Dto.Entities;

namespace Sale.Core.Domain.Saga.Commands
{
    public class ProcessPaymentCommand : ICommand
    {
        public Guid SaleId { get; set; }
        public decimal Valor { get; set; }
        public Guid PaymentRequestId { get; set; }
    }

    public class ReserveStockCommand : ICommand
    {
        public Guid SaleId { get; set; }
        public List<SaleItensDto> SaleItens { get; set; }
    }

    public class SaleStockConfirmedCommand : ICommand
    {
        public Guid SaleId { get; set; }
    }

    public class SaleStockInsufficienCommand : ICommand
    {
        public Guid SaleId { get; set; }
    }

    public class SalePaymentConfirmedCommand : ICommand
    {
        public Guid SaleId { get; set; }
    }

    public class SalePaymentCancelledCommand : ICommand
    {
        public Guid SaleId { get; set; }
    }
}
