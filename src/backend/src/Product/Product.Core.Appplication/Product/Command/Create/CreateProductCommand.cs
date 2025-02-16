using MediatR;

namespace Product.Core.Application.Products.Create
{
    public class CreateProductCommand : IRequest<CreateProductResult>
    {                
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }        
    }
}
