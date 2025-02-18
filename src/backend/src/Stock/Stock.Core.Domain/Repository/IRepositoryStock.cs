using Base.Core.Domain.Common;

namespace Stock.Core.Domain.Repository;

public interface IStockRepository : IRepositoryBase<StockCoreDomainEntitties.Stock>
{
    /// <summary>
    /// Returns the sale and itens of sale from the database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StockCoreDomainEntitties.Stock> GetByProductIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<StockCoreDomainEntitties.Stock>> GetListProductVyIdAsync(Guid id);

    Task<(IReadOnlyList<StockCoreDomainEntitties.Stock> stocks, int totalCount)> GetPagedAsync
    (
    int pageNumber,
    int pageSize,
    string orderBy,
    bool isDescending,
    CancellationToken cancellationToken
    );
}
