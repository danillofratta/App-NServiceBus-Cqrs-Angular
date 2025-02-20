using AutoMapper;

namespace Product.Core.Application.Products.Modify
{
    public class ModifyProductProfile : Profile
    {
        public ModifyProductProfile()
        {
            CreateMap<ModifyProductCommand, Product.Core.Domain.Entities.Product>();            
            CreateMap<Product.Core.Domain.Entities.Product, ModifyProductResult>();
        }
    }
}
