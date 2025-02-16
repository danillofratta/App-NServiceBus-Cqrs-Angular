namespace Stock.WebApi.Controllers.Command.AddProductStock
{
    public class AddProductStockRequest
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
