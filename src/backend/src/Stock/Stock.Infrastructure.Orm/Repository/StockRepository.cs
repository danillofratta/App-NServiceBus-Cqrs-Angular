using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Common;
using Shared.Infrastructure.Orm;
using Stock.Core.Domain.Repository;

namespace Stock.Infrastructure.Orm.Repository;

public class StockRepository : RepositoryBase<StockCoreDomainEntitties.Stock>, IStockRepository
{
    public StockRepository(DefaultDbContext defaultDbContext) : base(defaultDbContext)
    {
    }

    public override Task BeforeUpdateAsync(StockCoreDomainEntitties.Stock obj)
    {
        obj.UpdatedAt = DateTime.UtcNow;
        return base.BeforeUpdateAsync(obj);
    }

    public async Task<StockCoreDomainEntitties.Stock> GetByProductIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _DefaultDbContext.Stocks.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == id, cancellationToken);
    }

    public async Task<List<StockCoreDomainEntitties.Stock>> GetListProductVyIdAsync(Guid id)
    {
        return await _DefaultDbContext.Stocks.AsNoTracking().Where(x => x.ProductId == id).ToListAsync();
    }

    public async Task<(IReadOnlyList<StockCoreDomainEntitties.Stock> stocks, int totalCount)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string orderBy,
    bool isDescending,
    CancellationToken cancellationToken)
    {
        //IQueryable<Ambev.Sale.Core.Domain.Entities.Sale> query = _DefaultDbContext.Sales;

        IQueryable<StockCoreDomainEntitties.Stock> query = _DefaultDbContext.Stocks.AsQueryable();

        // Apply ordering if specified
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            query = orderBy.ToLower() switch
            {
                "productid" => isDescending
                    ? query.OrderByDescending(s => s.ProductId)
                    : query.OrderBy(s => s.ProductId),
                "productname" => isDescending
                    ? query.OrderByDescending(s => s.ProductName)
                    : query.OrderBy(s => s.ProductName),
                "quantity" => isDescending
                    ? query.OrderByDescending(s => s.Quantity)
                    : query.OrderBy(s => s.Quantity)
            };
        }
        else
        {
            // Default ordering if none specified
            query = query.OrderByDescending(s => s.CreatedAt);
        }


        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}

