using MediatR;
using FluentValidation;
using AutoMapper;

namespace Stock.Core.Application.Stock.AddProductStock
{
    public class AddProductStockHandler : IRequestHandler<AddProductStockCommand, AddProductStockResult>
    {
        private readonly Domain.Repository.IStockRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AddProductStockHandler(IMediator mediator, Domain.Repository.IStockRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<AddProductStockResult> Handle(AddProductStockCommand command, CancellationToken cancellationToken)
        {
            var validator = new AddProductStockCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (validationResult != null && !validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            //var record = _mapper.Map<Stock.Core.Domain.Entities.Stock>(command);
            var stock = await _repository.GetByProductIdAsync(command.ProductId);
            if (stock != null)
            {
                List<StockCoreDomainEntitties.Stock> liststock = await _repository.GetListProductVyIdAsync(stock.ProductId);
                stock.Quantity = command.Quantity + liststock.Sum(x => x.Quantity);

                await _repository.UpdateAsync(stock);
            }
            else
            {
                stock = new StockCoreDomainEntitties.Stock();
                stock.ProductId = command.ProductId;
                stock.ProductName = command.ProductName;
                stock.Price = command.Price;
                stock.Quantity = command.Quantity;
                await _repository.SaveAsync(stock);
            }

            return new AddProductStockResult
            {
                Id = stock.Id,
                ProductId = stock.ProductId,
                ProductName = stock.ProductName,
                Price = stock.Price,
                Quantity = stock.Quantity,
            };
        }
    }
}
