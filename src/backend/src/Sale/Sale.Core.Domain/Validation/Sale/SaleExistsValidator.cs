using FluentValidation;
using Sale.Core.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Core.Domain.Validation.Sale;

public class SaleExistsValidator : AbstractValidator<SaleCoreDomainEntities.Sale>
{
    private readonly ISaleRepository _repository;

    public SaleExistsValidator(ISaleRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Sale is required")
            .MustAsync(ExistInDatabase).WithMessage("Sale not found");          
    }

    private async Task<bool> ExistInDatabase(Guid id, CancellationToken cancellationToken)
    {
        var record = await _repository.GetByIdAsync(id);
        if (record != null) { return true; } else { return false; }
    }
}