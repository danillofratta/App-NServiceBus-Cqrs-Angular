using MediatR;
using FluentValidation;
using AutoMapper;
using Product.Core.Domain.Repository;


namespace Product.Core.Application.Products.Delete
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResult>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public DeleteProductHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new DeleteProductCommandValidator(_repository);
            var validationResult = await validator.ValidateAsync(command, cancellationToken);            
            if (validationResult != null && !validationResult.IsValid)                
                throw new ValidationException(validationResult.Errors);

            var record = await _repository.GetByIdAsync(command.Id);            
            
            var update = await _repository.DeleteAsync(record);
            
            return _mapper.Map<DeleteProductResult>(update); ;            
        }
    }
}
