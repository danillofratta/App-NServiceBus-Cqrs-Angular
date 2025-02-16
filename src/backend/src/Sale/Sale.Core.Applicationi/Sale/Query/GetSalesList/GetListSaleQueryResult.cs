using Sale.Core.Application.Sale.Dto;
using Sale.Core.Domain.Enum;

namespace Sale.Core.Application.Sale.Query.GetSalesList
{
    public class GetListSaleQueryResult
    {
        public Guid id { get; set; }
        public int Number { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public SaleStatus Status { get; set; } = SaleStatus.Create;
        public List<GetSaleItemDto> SaleItens { get; set; } = new();
    }
}
