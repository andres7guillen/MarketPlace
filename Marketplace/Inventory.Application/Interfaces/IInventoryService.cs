using CSharpFunctionalExtensions;
using Inventory.Application.DTOs;

namespace Inventory.Application.Interfaces;

public interface IInventoryService
{
    Task<Result<StockDto>> CreateStockAsync(Guid productId);
    Task<Result<StockDto>> UpdateStockAsync(Guid productId, int quantity);
    Task<Result<bool>> DeleteStockAsync(Guid productId);
    Task<Maybe<StockDto>> GetByProductIdAsync(Guid productId);
    Task<Result<List<StockDto>>> GetAllAsync();
}