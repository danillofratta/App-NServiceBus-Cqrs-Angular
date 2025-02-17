using AutoMapper;

namespace Stock.Core.Application.Stock.AddProductStock
{
    public class AddProductStockProfile : Profile
    {
        public AddProductStockProfile()
        {

            CreateMap<AddProductStockCommand, StockCoreDomainEntitties.Stock>();                          

            CreateMap<StockCoreDomainEntitties.Stock, AddProductStockResult>();
        }
    }
}
