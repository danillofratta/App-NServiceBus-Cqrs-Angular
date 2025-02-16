using Base.Core.Domain.Common;
using Sale.Core.Domain;

namespace Sale.Core.Domain.Repository;

public interface ISaleRepository : IRepositoryBase<SaleCoreDomainEntities.Sale>
{
    /// <summary>
    /// Returns the sale and itens of sale from the database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SaleCoreDomainEntities.Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns sales from the database
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="orderBy"></param>
    /// <param name="isDescending"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<(IReadOnlyList<SaleCoreDomainEntities.Sale> sales, int totalCount)> GetPagedAsync
        (
        int pageNumber,
        int pageSize,
        string orderBy,
        bool isDescending,
        CancellationToken cancellationToken
        );
}

