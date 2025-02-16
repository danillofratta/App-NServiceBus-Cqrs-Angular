using MediatR;
using Base.Common.Validation;

namespace Base.WebApi.Common;

public class ApiResponseEvent : ApiResponse, IEvent, INotification
{
    
}
