using Sale.Core.Domain.Contracts.Dto.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Core.Domain.Contracts.Event
{
    public class CreatedSaleItemEvent : IEvent
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public SaleItemStatusDto Status { get; set; } = SaleItemStatusDto.Create;
    }

}
