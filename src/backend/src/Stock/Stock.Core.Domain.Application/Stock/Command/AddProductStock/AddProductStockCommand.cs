using MediatR;

namespace Stock.Core.Application.Stock.AddProductStock
{
    public class AddProductStockCommand : IRequest<AddProductStockResult>
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
