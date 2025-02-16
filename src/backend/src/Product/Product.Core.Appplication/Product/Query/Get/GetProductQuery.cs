using MediatR;

namespace Product.Core.Application.Products.Get
{
    public class GetProductQuery : IRequest<GetProductQueryResult>
    {
        public Guid Id { get; set; }

        public GetProductQuery(Guid id)
        {
            Id = id;
        }

    }
}
