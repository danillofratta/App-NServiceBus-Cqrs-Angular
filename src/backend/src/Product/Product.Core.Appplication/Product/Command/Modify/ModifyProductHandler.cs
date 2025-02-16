using MediatR;
using FluentValidation;
using AutoMapper;
using Product.Core.Domain.Repository;

namespace Product.Core.Application.Products.Modify
{
    public class ModifyProductHandler : IRequestHandler<ModifyProductCommand, ModifyProductResult>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ModifyProductHandler(IMediator mediator, IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ModifyProductResult> Handle(ModifyProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new ModifyProductCommandValidator(_repository);
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (validationResult != null && !validationResult.IsValid)                
                throw new ValidationException(validationResult.Errors);

            var record = _mapper.Map<Product.Core.Domain.Entities.Product >(command);
            
            var update = await _repository.UpdateAsync(record);
            var result = _mapper.Map<ModifyProductResult>(update);

            return result;            
        }
    }
}
