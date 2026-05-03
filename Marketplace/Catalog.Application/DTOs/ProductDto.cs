namespace Catalog.Application.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    decimal Price
);
