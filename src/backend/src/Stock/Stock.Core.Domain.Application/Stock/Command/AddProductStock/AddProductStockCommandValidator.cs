using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;


namespace Stock.Core.Application.Stock.AddProductStock
{
    //TODO checj idproduct exists
    public class AddProductStockCommandValidator : AbstractValidator<AddProductStockCommand>
    {
        public AddProductStockCommandValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
        }
    }
}
