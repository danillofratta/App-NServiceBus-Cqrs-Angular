using FluentValidation;

namespace Sale.WebApi.Controllers.Sale.GetList;

public class GetLIstSaleRequestValidator : AbstractValidator<GetListSaleRequest>
{
    public GetLIstSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}
