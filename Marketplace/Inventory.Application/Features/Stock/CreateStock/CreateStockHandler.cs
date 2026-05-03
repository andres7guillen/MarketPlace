using CSharpFunctionalExtensions;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Stock.CreateStock;

public class CreateStockHandler
    : IRequestHandler<CreateStockCommand, Result<StockDto>>
{
    private readonly IInventoryService _service;

    public CreateStockHandler(IInventoryService service)
    {
        _service = service;
    }

    public async Task<Result<StockDto>> Handle(
        CreateStockCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var stock = await _service.CreateStockAsync(
                request.ProductId);

            return Result.Success<StockDto>(stock.Value);
        }
        catch (Exception ex)
        {
            return Result.Failure<StockDto>(ex.Message);
        }
    }
}
