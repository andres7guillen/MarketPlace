using CSharpFunctionalExtensions;
using Inventory.Application.DTOs;
using MediatR;

namespace Inventory.Application.Features.Stock.UpdateStock;

public record UpdateStockCommand(Guid ProductId, int Quantity) : IRequest<Result<StockDto>>;
