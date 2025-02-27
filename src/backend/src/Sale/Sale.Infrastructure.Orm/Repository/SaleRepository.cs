using Microsoft.EntityFrameworkCore;
using Sale.Core.Domain.Repository;
using Shared.Infrastructure.Common;
using Shared.Infrastructure.Orm;

namespace Sale.Infrastructure.Orm.Repository;

public class SaleRepository : RepositoryBase<SaleCoreDomainEntities.Sale>, ISaleRepository
{
    public SaleRepository(DefaultDbContext defaultDbContext) : base(defaultDbContext)
    {
    }

    public async Task<SaleCoreDomainEntities.Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _DefaultDbContext.Sales.AsNoTracking().Include(s => s.SaleItens).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<SaleCoreDomainEntities.Sale> sales, int totalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string orderBy,
        bool isDescending,
        CancellationToken cancellationToken)
    {
        //IQueryable<Ambev.Sale.Core.Domain.Entities.Sale> query = _DefaultDbContext.Sales;

        IQueryable<SaleCoreDomainEntities.Sale> query = _DefaultDbContext.Sales
                    .Include(x => x.SaleItens)
                    .AsQueryable();

        // Apply ordering if specified
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            query = orderBy.ToLower() switch
            {
                "number" => isDescending
                    ? query.OrderByDescending(s => s.Number)
                    : query.OrderBy(s => s.Number),
                "customername" => isDescending
                    ? query.OrderByDescending(s => s.CustomerName)
                    : query.OrderBy(s => s.CustomerName),
                "totalamount" => isDescending
                    ? query.OrderByDescending(s => s.TotalAmount)
                    : query.OrderBy(s => s.TotalAmount),
                "date" => isDescending
                    ? query.OrderByDescending(s => s.CreatedAt)
                    : query.OrderBy(s => s.CreatedAt),
                _ => query.OrderByDescending(s => s.CreatedAt) // Default ordering
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

