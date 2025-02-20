using AutoMapper;


namespace Product.Core.Application.Products.Get
{
    public class GetProductQueryProfile : Profile
    {
        public GetProductQueryProfile()
        {
            CreateMap<Product.Core.Domain.Entities.Product, GetProductQueryResult>();            
        }
    }
}
