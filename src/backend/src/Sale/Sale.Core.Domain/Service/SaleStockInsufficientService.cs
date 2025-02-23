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
    public class SaleStockInsufficientService : BaseService
    {
        private readonly ISaleRepository _salerepository;

        public SaleStockInsufficientService(ISaleRepository salerepository)
        {
            _salerepository = salerepository;
        }

        public async Task Process(Guid idsale)
        {
            try
            {
                var sale = await _salerepository.GetByIdAsync(idsale);
                if (sale != null && sale.Status != Enum.SaleStatus.StockInsufficient)
                {
                    sale.Status = Enum.SaleStatus.StockInsufficient;
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
                            Detail = "error save stock not insufficient"
                        }
                    );
                throw ex;
            }
        }
    }
}
