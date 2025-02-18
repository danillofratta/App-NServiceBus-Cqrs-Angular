using Base.Common.Validation;
using Base.Core.Domain.Service;
using Payment.Core.Domain.Enum;
using Payment.Core.Domain.Repository;


namespace Payment.Core.Domain.Services
{
    public class PaymentConfirmedService : BaseService
    {
        private readonly IPaymentRepository _PaymentRepository;

        public PaymentConfirmedService(IPaymentRepository paymentRepository) { 
            _PaymentRepository = paymentRepository;
        }

        public async Task Process(Guid idsale)
        {
            try
            {
                var sale = await _PaymentRepository.GetByIdSaleAsync(idsale);
                if (sale != null)
                {
                    sale.Status = PaymentStatus.Sucefull;
                    await _PaymentRepository.UpdateAsync(sale);
                }
            }
            catch (Exception ex)
            {
                this._ListError.Add
                    (
                        new ValidationErrorDetail
                        {
                            Error = ex.Message,
                            Detail = "error save payment confirmed"
                        }
                    );
                throw ex;
            }
        }
    }
}
