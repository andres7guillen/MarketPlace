using CSharpFunctionalExtensions;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Stock.GetAllStock;

public class GetAllStocksHandler
    : IRequestHandler<GetAllStocksQuery, Result<List<StockDto>>>
{
    private readonly IInventoryService _service;

    public GetAllStocksHandler(IInventoryService service)
    {
        _service = service;
    }

    public async Task<Result<List<StockDto>>> Handle(
        GetAllStocksQuery request,
        CancellationToken cancellationToken)
    {
        var stocks = await _service.GetAllAsync();

        return Result.Success<List<StockDto>>(stocks.Value);
    }
}