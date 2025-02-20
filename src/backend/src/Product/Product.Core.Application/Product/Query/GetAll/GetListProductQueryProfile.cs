using AutoMapper;

namespace Product.Core.Application.Products.GetList
{
    public class GetListProductQueryProfile : Profile
    {
        public GetListProductQueryProfile()
        {
            CreateMap<Product.Core.Domain.Entities.Product, GetListProductQueryResult>();
        }
    }
}
