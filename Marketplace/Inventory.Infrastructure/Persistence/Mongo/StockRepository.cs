using CSharpFunctionalExtensions;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Configuration;
using MongoDB.Driver;

namespace Inventory.Infrastructure.Persistence.Mongo;

public class StockRepository : IStockRepository
{
    private readonly MongoDbContext _context;

    public StockRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Maybe<Stock>> GetByProductIdAsync(Guid productId)
    {
        var stock = await _context.Stocks
            .Find(x => x.ProductId == productId)
            .FirstOrDefaultAsync();

        return stock == null
            ? Maybe<Stock>.None
            : Maybe<Stock>.From(stock);
    }

    public async Task<Result<List<Stock>>> GetAllAsync()
    {
        try
        {
            var stocks = await _context.Stocks
                .Find(_ => true)
                .ToListAsync();

            return Result.Success(stocks);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<Stock>>($"Error obteniendo stocks: {ex.Message}");
        }
    }

    public async Task<Result<Stock>> CreateAsync(Stock stock)
    {
        try
        {
            await _context.Stocks.InsertOneAsync(stock);
            return Result.Success(stock);
        }
        catch (Exception ex)
        {
            return Result.Failure<Stock>($"Error creando stock: {ex.Message}");
        }
    }

    public async Task<Result<Stock>> UpdateAsync(Stock stock)
    {
        try
        {
            var result = await _context.Stocks.ReplaceOneAsync(
                x => x.Id == stock.Id,
                stock);

            if (result.MatchedCount == 0)
                return Result.Failure<Stock>("Stock no encontrado");

            return Result.Success(stock);
        }
        catch (Exception ex)
        {
            return Result.Failure<Stock>($"Error actualizando stock: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(Guid productId)
    {
        try
        {
            var result = await _context.Stocks
                .DeleteOneAsync(x => x.ProductId == productId);

            if (result.DeletedCount == 0)
                return Result.Failure<bool>("Stock no encontrado");

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>($"Error eliminando stock: {ex.Message}");
        }
    }
}
