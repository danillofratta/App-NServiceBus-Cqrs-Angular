using FluentValidation;

namespace Stock.WebApi.Controllers.Query.GetList;

public class GetListStockRequestValidator : AbstractValidator<GetListStockRequest>
{
    public GetListStockRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
}
