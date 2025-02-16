using AutoMapper;
using MediatR;
using Stock.Core.Domain.Repository;


namespace Stock.Core.Application.Stock.Query.GetList;
public class GetListStockQueryHandler : IRequestHandler<GetListStockQuery, PagedResult<GetListStockQueryResult>>
{
    private readonly IStockRepository _repository;
    private readonly IMapper _mapper;

    public GetListStockQueryHandler(IStockRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<GetListStockQueryResult>> Handle(GetListStockQuery request, CancellationToken cancellationToken)
    {
        var (sales, totalCount) = await _repository.GetPagedAsync(
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            orderBy: request.OrderBy,
            isDescending: request.IsDescending,
            cancellationToken: cancellationToken
        );

        var dtos = _mapper.Map<List<GetListStockQueryResult>>(sales);

        return new PagedResult<GetListStockQueryResult>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}


