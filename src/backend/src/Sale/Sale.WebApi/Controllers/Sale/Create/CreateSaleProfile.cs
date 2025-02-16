using Sale.Core.Application.Sales.Create;
using AutoMapper;

namespace Sale.WebApi.Controllers.Sale.Create
{
    public class CreateSaleProfile : Profile
    {
       public CreateSaleProfile()
        {
            //CreateMap<CreateSaleRequest, CreateSaleCommand>();

            CreateMap<CreateSaleRequest, CreateSaleCommand>()
                .ForMember(dto => dto.SaleItens, conf => conf.MapFrom(ol => ol.SaleItens));

            CreateMap<CreateSaleResult, CreateSaleResponse>();
        }
    }
}
