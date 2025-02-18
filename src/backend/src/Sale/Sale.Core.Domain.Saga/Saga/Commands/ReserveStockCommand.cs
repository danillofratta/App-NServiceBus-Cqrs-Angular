using Sale.Core.Domain.Contracts.Dto.Entities;

namespace Sale.Core.Domain.Saga.Commands
{
    public class ReserveStockCommand : ICommand
    {
        public Guid SaleId { get; set; }
        public List<SaleItensDto> SaleItens { get; set; }
    }
}
