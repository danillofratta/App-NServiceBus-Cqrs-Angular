using Base.Core.Domain.Common;

namespace Product.Core.Domain.Repository;

public interface IProductRepository : IRepositoryBase<Product.Core.Domain.Entities.Product>
{
    Task<Product.Core.Domain.Entities.Product> GetByIdAsync(Guid id);

    Task<List<Product.Core.Domain.Entities.Product>> GetAllAsync();

    Task<List<Product.Core.Domain.Entities.Product>> GetByName(string name);

    Task AfterSaveAsync(Product.Core.Domain.Entities.Product obj);

    Task AfterUpdateAsync(Product.Core.Domain.Entities.Product obj);

    Task BeforeUpdateAsync(Product.Core.Domain.Entities.Product obj);

    Task AfterDeleteAsync(Product.Core.Domain.Entities.Product obj);

    Task<(IReadOnlyList<Product.Core.Domain.Entities.Product> products, int totalCount)> GetPagedAsync
        (
        int pageNumber,
        int pageSize,
        string orderBy,
        bool isDescending,
        CancellationToken cancellationToken
        );
}
