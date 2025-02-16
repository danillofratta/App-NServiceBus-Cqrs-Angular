using AutoMapper;
using Stock.Core.Application.Stock.Query.GetList;

namespace Stock.WebApi.Controllers.Query.GetList;

public class GetListStock : Profile
{
    public GetListStock()
    {
        CreateMap<GetListStockQueryResult, GetListStockResponse>();
    }
}

