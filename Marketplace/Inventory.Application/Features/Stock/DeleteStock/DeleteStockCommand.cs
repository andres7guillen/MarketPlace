using CSharpFunctionalExtensions;
using MediatR;

namespace Inventory.Application.Features.Stock.DeleteStock;

public record DeleteStockCommand(Guid ProductId) : IRequest<Result<bool>>;