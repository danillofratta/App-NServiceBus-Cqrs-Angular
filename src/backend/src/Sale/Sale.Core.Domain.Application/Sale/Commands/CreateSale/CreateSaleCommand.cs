using MediatR;
using Sale.Core.Domain.Contracts.Dto.Entities;
using Sale.Core.Domain.Enum;

namespace Sale.Core.Application.Sales.Create
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {        
        public string CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public string BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;

        public SaleStatus Status { get; set; } = SaleStatus.Create;

        public List<SaleItensDto> SaleItens { get; set; } = new();
    }
}
