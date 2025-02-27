using AutoMapper;


namespace Payment.Core.Domain.Application.Payment.Query.GetSalesList
{
    public class GetListPaymentQueryProfile : Profile
    {
        public GetListPaymentQueryProfile()
        {
            CreateMap<PaymentCoreDomainEntities.Payment, GetListPaymentQueryResult>();
        }
    }
}
