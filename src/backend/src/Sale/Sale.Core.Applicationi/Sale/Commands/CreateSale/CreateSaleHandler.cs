using MediatR;
using FluentValidation;
using AutoMapper;
using Sale.Core.Domain.Repository;
using NServiceBus;
using Shared.Dto.Event.Sale;

namespace Sale.Core.Application.Sales.Create
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;                
        private readonly IMediator _mediator;

        public CreateSaleHandler(IMediator mediator, ISaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;            
            _mediator = mediator;   
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            //TODO
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (validationResult != null && !validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var record = _mapper.Map<SaleCoreDomainEntities.Sale>(command);

            record.TotalAmount = record.SaleItens.Sum(x => x.TotalPrice);
            
            var created = await _repository.SaveAsync(record);
            var result = _mapper.Map<CreateSaleResult>(created);

            // Create a separate handler for publishing the event
            var eventMessage = new CreateSaleEventDto
            {
                Id = record.Id,
                //Number = record.Number,
                //CustomerId = command.CustomerId,
                //BranchId = command.BranchId,
                SaleItens = command.SaleItens
            };

            await _mediator.Publish(eventMessage, cancellationToken);

            //return result;
            return new CreateSaleResult();
        }
    }
}
