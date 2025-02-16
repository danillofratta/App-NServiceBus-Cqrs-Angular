using MediatR;

namespace Stock.Core.Application.Stock.Query.GetList;
 public class GetListStockQuery : IRequest<PagedResult<GetListStockQueryResult>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
}
