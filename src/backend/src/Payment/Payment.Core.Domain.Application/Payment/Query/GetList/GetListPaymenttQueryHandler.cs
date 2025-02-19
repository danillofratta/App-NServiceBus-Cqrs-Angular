
using AutoMapper;
using MediatR;
using Payment.Core.Domain.Repository;



namespace Payment.Core.Domain.Application.Payment.Query.GetSalesList;
public class GetListSaleQueryHandler : IRequestHandler<GetListPaymentQuery, PagedResult<GetListPaymentQueryResult>>
{
    private readonly IPaymentRepository _repository;
    private readonly IMapper _mapper;

    public GetListSaleQueryHandler(IPaymentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<GetListPaymentQueryResult>> Handle(GetListPaymentQuery request, CancellationToken cancellationToken)
    {
        var (sales, totalCount) = await _repository.GetPagedAsync(
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            orderBy: request.OrderBy,
            isDescending: request.IsDescending,
            cancellationToken: cancellationToken
        );

        var dtos = _mapper.Map<List<GetListPaymentQueryResult>>(sales);

        return new PagedResult<GetListPaymentQueryResult>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}


