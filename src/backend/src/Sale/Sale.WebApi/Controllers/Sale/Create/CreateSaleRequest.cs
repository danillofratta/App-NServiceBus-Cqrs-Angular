using Sale.Core.Domain.Contracts.Dto.Entities;

namespace Sale.WebApi.Controllers.Sale.Create
{
    public class CreateSaleRequest
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;

        public List<SaleItensDto> SaleItens { get; set; } = new();
    }
}
