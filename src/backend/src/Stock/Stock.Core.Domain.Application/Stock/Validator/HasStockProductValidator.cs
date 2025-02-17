//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FluentValidation;
//using FluentValidation.Validators;


//namespace Sale.Core.Application.Sales.Create
//{
//    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
//    {
//        public CreateSaleCommandValidator()
//        {
//            RuleFor(x => x.CustomerName).NotEmpty();
//            RuleFor(x => x.CustomerId).NotEmpty();
//            RuleFor(x => x.BranchId).NotEmpty();
//            RuleFor(x => x.BranchName).NotEmpty();

//            RuleFor(x => x.SaleItens)
//            .NotEmpty().WithMessage("The sale must contain at least one item.");

//            RuleForEach(x => x.SaleItens).ChildRules(items =>
//            {
//                items.RuleFor(i => i.ProductId)
//                    .NotEmpty().WithMessage("Product is required");

//                items.RuleFor(i => i.Quantity)
//                    .GreaterThan(0).WithMessage("The quantity must be greater than zero.");

//                items.RuleFor(i => i.UnitPrice)
//                    .GreaterThan(0).WithMessage("Price must be greater than zero.");
//            });
//        }
//    }
//}
