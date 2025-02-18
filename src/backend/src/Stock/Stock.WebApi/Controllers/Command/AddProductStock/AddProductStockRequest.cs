namespace Stock.WebApi.Controllers.Command.AddProductStock
{
    public class AddProductStockRequest
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
