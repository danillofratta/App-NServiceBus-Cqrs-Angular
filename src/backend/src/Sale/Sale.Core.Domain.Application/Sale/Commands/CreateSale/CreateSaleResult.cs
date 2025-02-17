using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Core.Application.Sales.Create
{
    public class CreateSaleResult : INotification
    {
        public Guid Id { get; set; }

        public int Number { get; set; }
    }
}
