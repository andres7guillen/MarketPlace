using Inventory.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Inventory.Infrastructure.Configuration;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb");
        var databaseName = configuration["MongoDbSettings:DatabaseName"];

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Stock> Stocks
        => _database.GetCollection<Stock>("stocks");
}
