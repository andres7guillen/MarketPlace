using CSharpFunctionalExtensions;
using Inventory.Application.DTOs;
using MediatR;

namespace Inventory.Application.Features.Stock.CreateStock;

public record CreateStockCommand(Guid ProductId, int Quantity) : IRequest<Result<StockDto>>;
