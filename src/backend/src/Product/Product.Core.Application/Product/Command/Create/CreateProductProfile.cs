using AutoMapper;


namespace Product.Core.Application.Products.Create
{
    public class CreateProductProfile : Profile
    {
        public CreateProductProfile()
        {

            CreateMap<CreateProductCommand, Product.Core.Domain.Entities.Product>();                          

            CreateMap<Product.Core.Domain.Entities.Product, CreateProductResult>();
        }
    }
}
