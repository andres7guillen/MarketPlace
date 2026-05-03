using Catalog.Domain.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Persistence.Mongo;

public interface IMongoDbContext
{
    IMongoCollection<Product> Products { get; }
}
