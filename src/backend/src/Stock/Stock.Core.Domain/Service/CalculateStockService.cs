using Base.Common.Validation;
using Stock.Core.Domain.Repository;
using Sale.Core.Domain.Contracts.Dto.Entities;
using Base.Core.Domain.Service;

namespace Stock.Core.Domain.Services
{
    //todo get parameter itens sale and quantity item
    public class CalculateStockService : BaseService
    {
        private readonly IStockRepository _stockRepository;

        public CalculateStockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        //TODO improve this using only validator class
        private async Task CheckItemInStock(string idproduct, decimal quantity)
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
            }            
        }

        public async Task<List<ValidationErrorDetail>> Calculate(List<SaleItensDto> listsaleitens)
        {
            //check if have quantity and products in stock
            foreach (var item in listsaleitens)
            {
                await this.CheckItemInStock(item.ProductId, item.Quantity);
            }

            //if dont have error
            if (this._ListError.Count <= 0)
            {
                foreach (var item in listsaleitens)
                {
                    //get product from stocl
                    var stockitem = await _stockRepository.GetByProductIdAsync(item.ProductId);
                    if (stockitem !=null)
                    {
                        //sibtrac item from stock
                        stockitem.Quantity = stockitem.Quantity - item.Quantity;
                        await _stockRepository.UpdateAsync(stockitem);
                    }                    
                }
            }

            return this._ListError;
        }

    }
}

