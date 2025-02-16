using MediatR;

namespace Product.Core.Application.Products.Delete
{
    public class DeleteProductCommand : IRequest<DeleteProductResult>
    {
        public Guid Id { get; set; }
    }
}
