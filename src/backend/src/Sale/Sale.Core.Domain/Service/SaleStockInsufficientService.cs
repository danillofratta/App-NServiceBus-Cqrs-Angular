using Base.Common.Validation;
using Base.Core.Domain.Service;
using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Specification;
using Sale.Core.Domain.Validation.Sale;
using FluentValidation;
using Sale.Core.Domain.Specification.Sale;

namespace Sale.Core.Domain.Service
{
    public class SaleStockInsufficientService : BaseService
    {
        private readonly ISaleRepository _salerepository;
        private readonly SaleExistsValidator _SaleExistsValidator;

        public SaleStockInsufficientService(ISaleRepository salerepository, SaleExistsValidator saleExistsValidator)
        {
            _salerepository = salerepository;
            _SaleExistsValidator = saleExistsValidator;
        }

        public async Task Process(Guid idsale)
        {
            try
            {
                var validator = new SaleExistsValidator(_salerepository);
                var validationResult = await validator.ValidateAsync(new SaleCoreDomainEntities.Sale { Id = idsale});
                if (validationResult != null && !validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);

                var sale = await _salerepository.GetByIdAsync(idsale);                    

                var canBeCancelled = new SaleCanBePaymentFailedSpecification().IsSatisfiedBy(sale);
                if (!canBeCancelled)
                    throw new ValidationException("The sale stock cannot be insufficient  at this stage.");

                if (sale != null && sale.Status != Enum.SaleStatus.StockInsufficient)
                {
                    sale.StockInsufficient();
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
