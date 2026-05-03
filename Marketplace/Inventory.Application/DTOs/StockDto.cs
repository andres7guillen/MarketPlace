namespace Inventory.Application.DTOs;

public record StockDto(
    Guid ProductId,
    int Quantity
);
