using Base.Common.Validation;
using Base.Core.Domain.Service;
using Payment.Core.Domain.Enum;
using Payment.Core.Domain.Repository;

namespace Payment.Core.Domain.Services
{
    public class PaymentCancelledService : BaseService
    {
        private readonly IPaymentRepository _PaymentRepository;

        public PaymentCancelledService(IPaymentRepository paymentRepository)
        {
            _PaymentRepository = paymentRepository;
        }

        public async Task Process(Guid idsale)
        {
            try
            {
                var sale = await _PaymentRepository.GetByIdSaleAsync(idsale);
                if (sale != null)
                {
                    sale.Status = PaymentStatus.Cancelled;
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
                            Detail = "error save cancelled confirmed"
                        }
                    );
                throw ex;
            }
        }
    }
}
