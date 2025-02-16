using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shared.Dto.Command.Sale;

public record CreateSaleItemDto
(
    string ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Discount,
    decimal TotalPrice
//todo
//SaleStatus Status = SaleStatus.Create
);
