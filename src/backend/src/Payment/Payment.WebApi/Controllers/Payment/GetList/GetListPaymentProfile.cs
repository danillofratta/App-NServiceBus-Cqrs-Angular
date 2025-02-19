using AutoMapper;
using Payment.Core.Domain.Application.Payment.Query.GetSalesList;

namespace Payment.WebApi.Controllers.Payment.GetList;

public class GetListPaymentProfile : Profile
{
    public GetListPaymentProfile()
    {
        CreateMap<GetListPaymentQueryResult, GetListPaymentResponse>();                   
    }
}

