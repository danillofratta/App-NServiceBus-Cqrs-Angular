namespace Base.Core.Domain.Common;

/// <summary>
/// TODO: add propertys to log user create, user update, ....
/// </summary>
public class BaseEntity 
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Delete logic like cancel
    /// </summary>
    public DateTime? DeletedAt { get; set; }    

    /// <summary>
    /// Register when update register
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    //public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    //{
    //    return Validator.ValidateAsync(this);
    //}

    //public int CompareTo(BaseEntity? other)
    //{
    //    if (other == null)
    //    {
    //        return 1;
    //    }

    //    return other!.Id.CompareTo(Id);
    //}
}
