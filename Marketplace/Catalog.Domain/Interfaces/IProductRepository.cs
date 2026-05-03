using Catalog.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Catalog.Domain.Interfaces;

public interface IProductRepository
{
    Task<Result> AddAsync(Product product);

    Task<Maybe<Product>> GetByIdAsync(Guid id);

    Task<Result> UpdateAsync(Product product);
}
