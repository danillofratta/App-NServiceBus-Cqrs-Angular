using Microsoft.EntityFrameworkCore;
using Payment.Core.Domain.Repository;
using Shared.Infrastructure.Common;
using Shared.Infrastructure.Orm;

namespace Payment.Infrastructure.Orm.Repository;

public class PaymentRepository : RepositoryBase<PaymentCoreDomainEntities.Payment>, IPaymentRepository
{

    public PaymentRepository( DefaultDbContext defaultDbContext) : base(defaultDbContext)
    {
    }



    public async override Task AfterDeleteAsync(PaymentCoreDomainEntities.Payment obj)
    {        

    }

    public async override Task AfterUpdateAsync(PaymentCoreDomainEntities.Payment obj)
    {

    }

    public async override Task AfterSaveAsync(PaymentCoreDomainEntities.Payment obj)
    {
    
    }

    public async Task<PaymentCoreDomainEntities.Payment> GetByIdSaleAsync(Guid SaleId, CancellationToken cancellationToken = default)
    {
        return await _DefaultDbContext.Payments.AsNoTracking().FirstOrDefaultAsync(x => x.SaleId == SaleId, cancellationToken);
    }

    public async Task<List<PaymentCoreDomainEntities.Payment>> GetAll()
    {
        return await _DefaultDbContext.Payments
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt) 
            .Take(100) 
            .ToListAsync();
    }

    public async Task<(IReadOnlyList<PaymentCoreDomainEntities.Payment> payments, int totalCount)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string orderBy,
    bool isDescending,
    CancellationToken cancellationToken)
    {
        //IQueryable<Ambev.Sale.Core.Domain.Entities.Sale> query = _DefaultDbContext.Sales;

        IQueryable<PaymentCoreDomainEntities.Payment> query = _DefaultDbContext.Payments.AsQueryable();

        // Apply ordering if specified
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            query = orderBy.ToLower() switch
            {
                "total" => isDescending
                    ? query.OrderByDescending(s => s.Total)
                    : query.OrderBy(s => s.Total),
                "status" => isDescending
                    ? query.OrderByDescending(s => s.Status)
                    : query.OrderBy(s => s.Status),
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

    public async Task<PaymentCoreDomainEntities.Payment> GetByPaymentRequestId(Guid paymentRequestId)
    {
        return await _DefaultDbContext.Payments.FirstOrDefaultAsync(x => x.PaymentRequestId == paymentRequestId);    
    }
}

