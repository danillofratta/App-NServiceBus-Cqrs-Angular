using FluentValidation;
using Product.Core.Domain.Repository;


namespace Product.Core.Application.Products.Modify
{
    public class ModifyProductCommandValidator : AbstractValidator<ModifyProductCommand>
    {
        private readonly IProductRepository _repository;

        public ModifyProductCommandValidator(IProductRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Product is required")
                .MustAsync(ExistInDatabase).WithMessage("Product not found");

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();            
        }

        private async Task<bool> ExistInDatabase(Guid id, CancellationToken cancellationToken)
        {
            var record = await _repository.GetByIdAsync(id);
            if (record != null) { return true; } else { return false; }
        }
    }
}
