
using Stock.Core.Domain.Repository;
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

        public async Task Process(Guid idproduct, decimal quantity)
        {
            //get product from stocl
            var stockitem = await _stockRepository.GetByProductIdAsync(idproduct);
            if (stockitem != null)
            {
                //sibtrac item from stock
                stockitem.Quantity = stockitem.Quantity - quantity;
                await _stockRepository.UpdateAsync(stockitem);
            }

        }

    }
}

