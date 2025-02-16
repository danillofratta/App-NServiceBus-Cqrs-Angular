using AutoMapper;

namespace Stock.Core.Application.Stock.Query.GetList
{
    public class GetListStockQueryProfile : Profile
    {
        public GetListStockQueryProfile()
        {
            CreateMap<StockCoreDomainEntitties.Stock, GetListStockQueryResult>();
        }
    }
}
