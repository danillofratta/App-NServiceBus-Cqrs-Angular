using Base.Core.Domain.Service;
using FluentValidation;
using MediatR;
using Sale.Core.Domain.Repository;
using Sale.Core.Domain.Specification;
using Sale.Core.Domain.Specification.Sale;
using Sale.Core.Domain.Validation.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Core.Domain.Service;

public class SaleCancelledService : BaseService
{
    private readonly ISaleRepository _repository;
    private readonly SaleExistsValidator _SaleExistsValidator;

    public SaleCancelledService(ISaleRepository repository, SaleExistsValidator saleExistsValidator)
    {
        _repository = repository;
        _SaleExistsValidator = saleExistsValidator;
    }

    public async Task<bool> Process(Guid idsale)
    {
        var validator = new SaleExistsValidator(_repository);
        var validationResult = await validator.ValidateAsync(new SaleCoreDomainEntities.Sale { Id = idsale });
        if (validationResult != null && !validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _repository.GetByIdAsync(idsale);

        var canBeCancelled = new SaleCanBeCancelledSpecification().IsSatisfiedBy(sale);
        if (!canBeCancelled)
            throw new ValidationException("The sale cannot be cancelled at this stage.");

        sale.SaleCancelled();
        await _repository.UpdateAsync(sale);

        return true;
    }
}

