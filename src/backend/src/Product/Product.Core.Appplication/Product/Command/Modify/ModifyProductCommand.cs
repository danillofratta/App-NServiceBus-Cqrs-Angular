using MediatR;

namespace Product.Core.Application.Products.Modify
{
    public class ModifyProductCommand : IRequest<ModifyProductResult>
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
