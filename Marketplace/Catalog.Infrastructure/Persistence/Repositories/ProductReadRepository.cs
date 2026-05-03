using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using Catalog.Infrastructure.Persistence.Mongo;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Persistence.Repositories;

public class ProductReadRepository : IProductReadRepository
{
    private readonly IMongoDbContext _context;

    public ProductReadRepository(IMongoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _context.Products
            .Find(_ => true)
            .ToListAsync();

        return products.Select(p =>
            new ProductDto(p.Id, p.Name, p.Price));
    }
}
