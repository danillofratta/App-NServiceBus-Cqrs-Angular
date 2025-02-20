using MediatR;


namespace Product.Core.Application.Products.GetList;
 public class GetListProductQuery : IRequest<PagedResult<GetListProductQueryResult>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
}
