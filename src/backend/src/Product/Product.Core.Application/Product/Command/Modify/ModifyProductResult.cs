using MediatR;


namespace Product.Core.Application.Products.Modify
{
    public class ModifyProductResult : INotification
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
