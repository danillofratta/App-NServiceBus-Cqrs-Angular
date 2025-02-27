using Base.Common.Validation;
using Base.Core.Domain.Service;
using Payment.Core.Domain.Enum;
using Payment.Core.Domain.Repository;
using System;
using System.Linq;

namespace Payment.Core.Domain.Services
{
    public class PaymentCreateService : BaseService
    {
        private readonly IPaymentRepository _PaymentRepository;

        public PaymentCreateService(IPaymentRepository paymentRepository)
        {
            _PaymentRepository = paymentRepository;
        }

        public async Task<PaymentCoreDomainEntities.Payment> Process(Guid idsale, decimal total)
        {
            try
            {
                var payment = new PaymentCoreDomainEntities.Payment
                {
                    SaleId = idsale,
                    Status = PaymentStatus.Create,
                    Total = total
                };
                return await _PaymentRepository.SaveAsync(payment);
            }
            catch (Exception ex)
            {
                this._ListError.Add
                    (
                        new ValidationErrorDetail
                        {
                            Error = ex.Message,
                            Detail = "error save payment"
                        }
                    );
                throw ex;
            }
        }
    }
}
