using AutoMapper;
using MediatR;
using Product.Core.Domain.Repository;


namespace Product.Core.Application.Products.GetList;
public class GetListProductQueryHandler : IRequestHandler<GetListProductQuery, PagedResult<GetListProductQueryResult>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetListProductQueryHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<GetListProductQueryResult>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
    {
        var (Products, totalCount) = await _repository.GetPagedAsync(
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            orderBy: request.OrderBy,
            isDescending: request.IsDescending,
            cancellationToken: cancellationToken
        );

        var dtos = _mapper.Map<List<GetListProductQueryResult>>(Products);

        return new PagedResult<GetListProductQueryResult>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}


