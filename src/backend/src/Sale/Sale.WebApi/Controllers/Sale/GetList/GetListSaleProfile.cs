using AutoMapper;
using Sale.Core.Application.Sale.Query.GetSalesList;

namespace Sale.WebApi.Controllers.Sale.GetList;

public class GetListSaleProfile : Profile
{
    public GetListSaleProfile()
    {
        CreateMap<GetListSaleQueryResult, GetListSaleResponse>()
                   .ForMember(dto => dto.SaleItens, conf => conf.MapFrom(ol => ol.SaleItens));
    }
}

