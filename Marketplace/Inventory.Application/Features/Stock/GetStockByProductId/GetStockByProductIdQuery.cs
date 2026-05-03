using CSharpFunctionalExtensions;
using Inventory.Application.DTOs;
using MediatR;

namespace Inventory.Application.Features.Stock.GetStockByProductId;

public record GetStockByProductIdQuery(Guid ProductId) : IRequest<Result<StockDto>>;