namespace Stock.Core.Application.Stock.Query.GetList
{
    public class GetListStockQueryResult
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
