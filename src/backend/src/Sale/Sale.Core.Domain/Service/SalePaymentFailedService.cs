using Base.Common.Validation;
using Base.Core.Domain.Service;
using Sale.Core.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Core.Domain.Service
{
    public class SalePaymentFailedService : BaseService
    {
        private readonly ISaleRepository _salerepository;

        public SalePaymentFailedService(ISaleRepository salerepository)
        {
            _salerepository = salerepository;
        }

        public async Task Process(Guid idsale)
        {
            try
            {
                var sale = await _salerepository.GetByIdAsync(idsale);
                if (sale != null)
                {                
                    sale.Status = Enum.SaleStatus.PaymentCancelled;
                    await _salerepository.UpdateAsync(sale);
                }
            }
            catch (Exception ex)
            {
                this._ListError.Add
                    (
                        new ValidationErrorDetail
                        {
                            Error = ex.Message,
                            Detail = "error save payment not cancelled"
                        }
                    );
                throw ex;
            }
        }

    }
}
