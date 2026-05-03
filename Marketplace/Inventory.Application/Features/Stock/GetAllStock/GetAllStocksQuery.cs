using CSharpFunctionalExtensions;
using Inventory.Application.DTOs;
using MediatR;

namespace Inventory.Application.Features.Stock.GetAllStock;

public record GetAllStocksQuery() : IRequest<Result<List<StockDto>>>;
