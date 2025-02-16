using Sale.Core.Domain.Enum;

namespace Sale.Core.Application.Sale.Dto;

public record ModifySaleItemDto
(
    Guid SaleId,
    Guid Id,
    string ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Discount,
    decimal TotalPrice,
    SaleStatus Status
);
