
using AutoMapper;
using MediatR;
using Sale.Core.Domain.Repository;


namespace Sale.Core.Application.Sale.Query.GetSalesList;
public class GetListSaleQueryHandler : IRequestHandler<GetListSaleQuery, PagedResult<GetListSaleQueryResult>>
{
    private readonly ISaleRepository _repository;
    private readonly IMapper _mapper;

    public GetListSaleQueryHandler(ISaleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<GetListSaleQueryResult>> Handle(GetListSaleQuery request, CancellationToken cancellationToken)
    {
        var (sales, totalCount) = await _repository.GetPagedAsync(
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            orderBy: request.OrderBy,
            isDescending: request.IsDescending,
            cancellationToken: cancellationToken
        );

        var dtos = _mapper.Map<List<GetListSaleQueryResult>>(sales);

        return new PagedResult<GetListSaleQueryResult>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}


