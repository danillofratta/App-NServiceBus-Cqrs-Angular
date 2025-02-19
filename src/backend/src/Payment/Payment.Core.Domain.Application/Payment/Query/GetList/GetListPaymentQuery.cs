using MediatR;


namespace Payment.Core.Domain.Application.Payment.Query.GetSalesList;
public class GetListPaymentQuery : IRequest<PagedResult<GetListPaymentQueryResult>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
}
