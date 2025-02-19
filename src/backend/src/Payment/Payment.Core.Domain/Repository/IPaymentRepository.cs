using Base.Core.Domain.Common;

namespace Payment.Core.Domain.Repository;

public interface IPaymentRepository : IRepositoryBase<PaymentCoreDomainEntities.Payment>
{
    /// <summary>
    /// Returns the sale and itens of sale from the database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PaymentCoreDomainEntities.Payment> GetByIdSaleAsync(Guid SaleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns sales from the database
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="orderBy"></param>
    /// <param name="isDescending"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<PaymentCoreDomainEntities.Payment>> GetAll();

    Task<(IReadOnlyList<PaymentCoreDomainEntities.Payment> payments, int totalCount)> GetPagedAsync
    (
    int pageNumber,
    int pageSize,
    string orderBy,
    bool isDescending,
    CancellationToken cancellationToken
    );

}

