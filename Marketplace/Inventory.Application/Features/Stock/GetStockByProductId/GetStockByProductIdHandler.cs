using CSharpFunctionalExtensions;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Stock.GetStockByProductId;

public class GetStockByProductIdHandler
    : IRequestHandler<GetStockByProductIdQuery, Result<StockDto>>
{
    private readonly IInventoryService _service;

    public GetStockByProductIdHandler(IInventoryService service)
    {
        _service = service;
    }

    public async Task<Result<StockDto>> Handle(
        GetStockByProductIdQuery request,
        CancellationToken cancellationToken)
    {
        var stock = await _service.GetByProductIdAsync(request.ProductId);

        if (stock.HasNoValue)
            return Result.Failure<StockDto>("Stock not found");

        return Result.Success<StockDto>(stock.Value);
    }
}
