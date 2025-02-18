using Base.Common.Validation;
using Stock.Core.Domain.Repository;
using Base.Core.Domain.Service;

namespace Stock.Core.Domain.Services
{
    //todo get parameter itens sale and quantity item
    public class CheckItemInStockService : BaseService
    {
        private readonly IStockRepository _stockRepository;

        public CheckItemInStockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<bool> Process(Guid idproduct, decimal quantity)
        {
            var stockitem = await _stockRepository.GetByProductIdAsync(idproduct);
            if (stockitem != null)
            {
                //todo messagem and return error to sale.api
                if (quantity > stockitem.Quantity)
                {
                    this._ListError.Add
                        (
                            new Base.Common.Validation.ValidationErrorDetail
                            {
                                Detail = $"Product {idproduct} have {stockitem.Quantity} in stock and sale {quantity}",
                                Error = $"Product {idproduct} dont' have quantity request in stock"
                            }
                        );

                    return false;
                }
            }
            else
            {
                this._ListError.Add
                (
                    new Base.Common.Validation.ValidationErrorDetail
                    {
                        Detail = $"Product {idproduct} not found in stock",
                        Error = $"Product {idproduct} not found in stock"
                    }
                );

                return false;
            }   
            
            return true;
        }        

    }
}

