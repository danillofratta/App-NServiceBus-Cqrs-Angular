using MediatR;


namespace Sale.Core.Application.Sale.Query.GetSalesList;
 public class GetListSaleQuery : IRequest<PagedResult<GetListSaleQueryResult>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
}
