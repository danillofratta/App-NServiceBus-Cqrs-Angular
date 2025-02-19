using Microsoft.EntityFrameworkCore;
using Payment.Core.Domain.Application.Payment.Service;
using Payment.Core.Domain.Repository;
using Shared.Infrasctructure;
using Shared.Infrastructure;

namespace Payment.Infrastructure.Orm.Repository;

public class PaymentRepository : RepositoryBase<PaymentCoreDomainEntities.Payment>, IPaymentRepository
{
    //private readonly PaymentNotificationService _notificationService;

    //public PaymentRepository(DefaultDbContext defaultDbContext, PaymentNotificationService notificationService) : base(defaultDbContext)
    //{
    //    _notificationService = notificationService;        
    //}

    private readonly SignalRPaymentNotificationService _notificationService;

    public PaymentRepository(SignalRPaymentNotificationService notificationService, DefaultDbContext defaultDbContext) : base(defaultDbContext)
    {
        _notificationService = notificationService;
    }

    //public PaymentRepository(DefaultDbContext defaultDbContext) : base(defaultDbContext)
    //{

    //}

    public async override Task AfterDeleteAsync(PaymentCoreDomainEntities.Payment obj)
    {        
        //await _notificationService.NotifyPaymentsUpdated();
    }

    public async override Task AfterUpdateAsync(PaymentCoreDomainEntities.Payment obj)
    {
        //await _notificationService.NotifyPaymentsUpdated();
    }

    public async override Task AfterSaveAsync(PaymentCoreDomainEntities.Payment obj)
    {
        //await _notificationService.NotifyPaymentsUpdated();
        var payments = await GetAll();
        await _notificationService.NotifyPaymentsUpdated(payments);
        //await _hubContext.Clients.All.SendAsync("GetListPayment", payments);        
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
}

