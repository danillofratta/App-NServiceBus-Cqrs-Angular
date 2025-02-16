
using AutoMapper;
using Stock.Core.Application.Stock.AddProductStock;

namespace Stock.WebApi.Controllers.Command.AddProductStock
{
    public class AddProductStockProfile : Profile
    {
        public AddProductStockProfile()
        {
            CreateMap<AddProductStockRequest, AddProductStockCommand>();
            CreateMap<AddProductStockResult, AddProductStockResponse>();
        }
    }
}
