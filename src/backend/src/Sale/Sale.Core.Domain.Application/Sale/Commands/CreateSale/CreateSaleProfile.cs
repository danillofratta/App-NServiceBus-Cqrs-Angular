using AutoMapper;
using Sale.Core.Domain.Contracts.Dto.Entities;

namespace Sale.Core.Application.Sales.Create
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
        
            CreateMap<CreateSaleCommand, SaleCoreDomainEntities.Sale>()
                   .ForMember(dto => dto.SaleItens, conf => conf.MapFrom(ol => ol.SaleItens));                    

            CreateMap<SaleItensDto, SaleCoreDomainEntities.SaleItens>();

            CreateMap<SaleCoreDomainEntities.Sale, CreateSaleResult>();
        }
    }
}
