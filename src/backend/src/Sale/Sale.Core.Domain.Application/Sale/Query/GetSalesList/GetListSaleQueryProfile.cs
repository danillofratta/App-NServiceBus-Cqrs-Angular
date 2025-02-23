using AutoMapper;
using Sale.Core.Domain.Application.Sale.Query.GetSalesList.Dto;
using SaleCoreDomainEntities;

namespace Sale.Core.Application.Sale.Query.GetSalesList
{
    public class GetListSaleQueryProfile : Profile
    {
        public GetListSaleQueryProfile()
        {
            CreateMap<SaleCoreDomainEntities.Sale, GetListSaleQueryResult>()
                   .ForMember(dto => dto.SaleItens, conf => conf.MapFrom(ol => ol.SaleItens));

            CreateMap<SaleItens, GetSaleItemDto>();
        }
    }
}
