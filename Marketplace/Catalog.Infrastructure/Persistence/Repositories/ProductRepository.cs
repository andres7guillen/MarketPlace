using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Catalog.Domain.Outbox;
using Catalog.Infrastructure.Outbox;
using Catalog.Infrastructure.Persistence.Mongo;
using CSharpFunctionalExtensions;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoDbContext _context;
    private readonly IMongoCollection<OutboxMessage> _outboxCollection;
    private readonly IDomainEventDispatcher _dispatcher;

    public ProductRepository(
    IMongoDbContext context,
    IDomainEventDispatcher dispatcher,
    IMongoCollection<OutboxMessage> outboxCollection)
    {
        _context = context;
        _dispatcher = dispatcher;
        _outboxCollection = outboxCollection;
    }

    public async Task<Result> AddAsync(Product product)
    {
        try
        {
            await _context.Products.InsertOneAsync(product);
            var domainEvents = product.DomainEvents.ToList();
            var outboxMessages = domainEvents
                .Select(OutboxMessageFactory.Create)
                .ToList();

            if (outboxMessages.Any())
            {
                await _outboxCollection.InsertManyAsync(outboxMessages);
            }

            await _dispatcher.DispatchAsync(domainEvents);

            product.ClearDomainEvents();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Maybe<Product>> GetByIdAsync(Guid id)
    {
        var product = await _context.Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();

        return product == null
            ? Maybe<Product>.None
            : Maybe<Product>.From(product);
    }

    public async Task<Result> UpdateAsync(Product product)
    {
        try
        {
            var result = await _context.Products.ReplaceOneAsync(
                x => x.Id == product.Id,
                product);

            if (!result.IsAcknowledged)
                return Result.Failure("Update failed");

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

}