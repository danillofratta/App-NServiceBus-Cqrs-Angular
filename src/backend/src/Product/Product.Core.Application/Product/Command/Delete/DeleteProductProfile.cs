using AutoMapper;

namespace Product.Core.Application.Products.Delete
{
    public class DeleteProductProfile : Profile
    {
        public DeleteProductProfile()
        {       
            CreateMap<DeleteProductCommand, Product.Core.Domain.Entities.Product >();
            CreateMap<Product.Core.Domain.Entities.Product, DeleteProductResult>();            
        }
    }
}
