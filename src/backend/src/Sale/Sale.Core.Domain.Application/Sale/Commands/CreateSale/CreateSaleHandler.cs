using MediatR;
using FluentValidation;
using AutoMapper;
using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Contracts.Event;
using Rebus.Bus;
using Base.Infrastructure.Messaging;

namespace Sale.Core.Application.Sales.Create
{
    //todo add Imessage do nservice bus aqui 
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBus _bus;

        public CreateSaleHandler(
            IMessageBus bus,
            ISaleRepository repository, 
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _bus = bus;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (validationResult != null && !validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var record = _mapper.Map<SaleCoreDomainEntities.Sale>(command);

            record.TotalAmount = record.SaleItens.Sum(x => x.TotalPrice);

            var created = await _repository.SaveAsync(record);
            var result = _mapper.Map<CreateSaleResult>(created);

            var eventMessage = new SaleCreatedEvent
            {
                SaleId = record.Id,
                SaleItens = command.SaleItens
            };

            await _bus.PublishAsync(eventMessage, cancellationToken);

            return new CreateSaleResult();
        }
    }    
}
