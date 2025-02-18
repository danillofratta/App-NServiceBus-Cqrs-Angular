using MediatR;
using FluentValidation;
using AutoMapper;
using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Contracts.Event;
using NServiceBus;
using Sale.Core.Domain.Saga.Events;
using System.Security.Cryptography.X509Certificates;

namespace Sale.Core.Application.Sales.Create
{
    //com evento simples sem ser saga. Precisa descomentar stock e sale nservicebus config
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMessageSession _MessageSession;

        public CreateSaleHandler(IMessageSession message, ISaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _MessageSession = message;
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

            // Create a separate handler for publishing the event
            var eventMessage = new SaleCreatedEvent//CreatedSaleEvent
            {
                SaleId = record.Id,
                SaleItens = command.SaleItens
            };

            await _MessageSession.Publish(eventMessage, cancellationToken);

            //return result;
            return new CreateSaleResult();
        }
    }
}
