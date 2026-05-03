using CSharpFunctionalExtensions;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Stock.UpdateStock;

public class UpdateStockHandler
    : IRequestHandler<UpdateStockCommand, Result<StockDto>>
{
    private readonly IInventoryService _service;

    public UpdateStockHandler(IInventoryService service)
    {
        _service = service;
    }

    public async Task<Result<StockDto>> Handle(
        UpdateStockCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var stock = await _service.UpdateStockAsync(
                request.ProductId,
                request.Quantity);

            return Result.Success<StockDto>(stock.Value);
        }
        catch (Exception ex)
        {
            return Result.Failure<StockDto>(ex.Message);
        }
    }
}