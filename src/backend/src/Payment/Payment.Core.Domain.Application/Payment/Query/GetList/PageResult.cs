using System;
using System.Linq;

namespace Payment.Core.Domain.Application.Payment.Query.GetSalesList;
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
}
