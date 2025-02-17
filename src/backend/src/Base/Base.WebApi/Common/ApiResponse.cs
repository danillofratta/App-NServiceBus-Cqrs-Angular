using MediatR;
using Base.Common.Validation;

namespace Base.WebApi;

public class ApiResponse 
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<ValidationErrorDetail> Errors { get; set; } = [];
}
