using System.Collections.Generic;

namespace Base.Core.Domain.Common;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    /// <summary>
    /// call after save object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    Task AfterSaveAsync(TEntity obj);
    /// <summary>
    /// call after update object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    Task AfterUpdateAsync(TEntity obj);
    /// <summary>
    /// call after delete object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    Task AfterDeleteAsync(TEntity obj);
    /// <summary>
    /// call before save object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    Task BeforeSaveAsync(TEntity obj);
    /// <summary>
    /// call before update object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    Task BeforeUpdateAsync(TEntity obj);
    /// <summary>
    /// call before delete object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    Task BeforeDeleteAsync(TEntity obj);

    Task<TEntity> SaveAsync(TEntity obj);
    Task<TEntity> UpdateAsync(TEntity obj);
    Task<TEntity> DeleteAsync(TEntity obj);
}

