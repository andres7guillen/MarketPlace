using CSharpFunctionalExtensions;
using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces;

public interface IStockRepository
{
    Task<Result<Stock>> CreateAsync(Stock stock);
    Task<Maybe<Stock>> GetByProductIdAsync(Guid productId);
    Task<Result<List<Stock>>> GetAllAsync();
    Task<Result<Stock>> UpdateAsync(Stock stock);
    Task<Result<bool>> DeleteAsync(Guid productId);
}
