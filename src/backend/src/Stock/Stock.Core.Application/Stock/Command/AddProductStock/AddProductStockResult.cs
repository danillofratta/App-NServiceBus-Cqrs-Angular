using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Application.Stock.AddProductStock
{
    public class AddProductStockResult : INotification
    {
        public Guid Id { get; set; }

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
