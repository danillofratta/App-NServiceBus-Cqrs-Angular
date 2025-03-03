using Base.Core.Domain.Specification;
using Sale.Core.Domain.Enum;
using SaleCoreDomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Core.Domain.Specification.Sale
{
    public class SaleCanBeCancelledSpecification : BaseSpecification<SaleCoreDomainEntities.Sale>
    {
        public override bool IsSatisfiedBy(SaleCoreDomainEntities.Sale entity)
        {
            return entity.Status == SaleStatus.Create &&
                entity.Status == SaleStatus.StockInsufficient &&
                entity.Status == SaleStatus.StockConfirmed &&
                entity.Status == SaleStatus.PaymentCancelled &&
                entity.Status == SaleStatus.PaymentWaiting;
        }
    }
}
