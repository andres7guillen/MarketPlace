using Contracts.Events;
using CSharpFunctionalExtensions;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly IStockRepository _repository;

    public InventoryService(IStockRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<StockDto>> CreateStockAsync(Guid productId)
    {
        if (productId == Guid.Empty)
            return Result.Failure<StockDto>("ProductId inválido");

        var existing = await _repository.GetByProductIdAsync(productId);

        if (existing.HasValue)
        {
            Console.WriteLine($"⚠️ Stock ya existe para {productId}");
            return Result.Success(MapToDto(existing.Value));
        }

        var stock = Stock.Create(productId);        

        await _repository.CreateAsync(stock);

        Console.WriteLine($"💾 Stock creado: {productId}");

        return Result.Success(MapToDto(stock));
    }
   

    public async Task<Result<bool>> DeleteStockAsync(Guid productId)
    {
        var stock = await _repository.GetByProductIdAsync(productId);

        if (stock == null)
            return Result.Failure<bool>("Stock no encontrado");

        await _repository.DeleteAsync(productId);

        return Result.Success(true);
    }

    public async Task<Result<List<StockDto>>> GetAllAsync()
    {
        var stocks = await _repository.GetAllAsync();

        var result = stocks.Value
            .Select(MapToDto)
            .ToList();

        return Result.Success(result);
    }

    public async Task<Maybe<StockDto>> GetByProductIdAsync(Guid productId)
    {
        var stock = await _repository.GetByProductIdAsync(productId);

        if (stock == null)
            return Maybe<StockDto>.None;

        return Maybe<StockDto>.From(MapToDto(stock.Value));
    }

    public async Task<Result<StockDto>> UpdateStockAsync(Guid productId, int quantity)
    {
        var stock = await _repository.GetByProductIdAsync(productId);

        if (stock.HasNoValue)
            return Result.Failure<StockDto>("Stock no encontrado");

        stock.Value.UpdateQuantity(quantity);

        await _repository.UpdateAsync(stock.Value);

        return Result.Success(MapToDto(stock.Value));
    }

    private static StockDto MapToDto(Stock stock)
    {
        return new StockDto(stock.ProductId, stock.Quantity);
       
    }

}
