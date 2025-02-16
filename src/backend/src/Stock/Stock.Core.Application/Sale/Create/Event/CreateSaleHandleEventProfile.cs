using AutoMapper;
using Shared.Dto.Entities;
using Shared.Dto.Event.Sale;

namespace Stock.Core.Application.Stock.AddProductStock
{
    public class CreateSaleHandleEventProfile : Profile
    {
        public CreateSaleHandleEventProfile()
        {
            CreateMap<CreateSaleEventDto, SaleItensDto>();
        }
    }
}
