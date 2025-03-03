using FluentValidation;

namespace Sale.Core.Domain.Validation.Sale;

/// <summary>
/// Performs basic entity validations
/// </summary>
public class SaleBasicValidator : AbstractValidator<SaleCoreDomainEntities.Sale>
{
    public SaleBasicValidator()
    {
        RuleFor(s => s.BranchId)
            .NotEmpty().WithMessage("BranchId is required.");

        RuleFor(s => s.BranchName)
            .NotEmpty().WithMessage("BranchName is required.")
            .MaximumLength(200).WithMessage("BranchName must be at most 200 characters.");

        RuleFor(s => s.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(s => s.CustomerName)
            .NotEmpty().WithMessage("CustomerName is required.")
            .MaximumLength(200).WithMessage("CustomerName must be at most 200 characters.");

        RuleFor(s => s.SaleItens)
            .NotEmpty().WithMessage("A sale must have at least one item.");

        RuleFor(s => s.TotalAmount)
            .GreaterThan(0).WithMessage("TotalAmount must be greater than zero.");
    }
}
