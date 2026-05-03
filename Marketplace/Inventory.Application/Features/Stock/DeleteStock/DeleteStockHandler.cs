using CSharpFunctionalExtensions;
using Inventory.Application.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Stock.DeleteStock;

public class DeleteStockHandler
    : IRequestHandler<DeleteStockCommand, Result<bool>>
{
    private readonly IInventoryService _service;

    public DeleteStockHandler(IInventoryService service)
    {
        _service = service;
    }

    public async Task<Result<bool>> Handle(
        DeleteStockCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteStockAsync(request.ProductId);

            return Result.Success<bool>(deleted.Value);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>(ex.Message);
        }
    }
}