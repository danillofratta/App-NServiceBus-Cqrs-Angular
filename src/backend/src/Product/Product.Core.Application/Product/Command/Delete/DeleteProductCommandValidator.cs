using FluentValidation;
using Product.Core.Domain.Repository;


namespace Product.Core.Application.Products.Delete
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        private readonly IProductRepository _repository;
        public DeleteProductCommandValidator(IProductRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Product is required")
                .MustAsync(ExistInDatabase).WithMessage("Product not found");
        }
        private async Task<bool> ExistInDatabase(Guid id, CancellationToken cancellationToken)
        {
            var record = await _repository.GetByIdAsync(id);
            if (record != null) { return true; } else { return false; }
        }
    }
}
