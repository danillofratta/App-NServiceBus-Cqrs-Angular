using Microsoft.EntityFrameworkCore;
using Payment.Core.Domain.Repository;
using Shared.Infrasctructure;
using Shared.Infrastructure;

namespace Payment.Infrastructure.Orm.Repository;

public class PaymentRepository : RepositoryBase<PaymentCoreDomainEntities.Payment>, IPaymentRepository
{
    public PaymentRepository(DefaultDbContext defaultDbContext) : base(defaultDbContext)
    {
    }    

    public async Task<PaymentCoreDomainEntities.Payment> GetByIdSaleAsync(Guid SaleId, CancellationToken cancellationToken = default)
    {
        return await _DefaultDbContext.Payments.AsNoTracking().FirstOrDefaultAsync(x => x.SaleId == SaleId, cancellationToken);
    }

    public async Task<List<PaymentCoreDomainEntities.Payment>> GetAll()
    {
        return await _DefaultDbContext.Payments.AsNoTracking().ToListAsync();
    }
}

