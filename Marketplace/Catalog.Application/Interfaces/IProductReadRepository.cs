using Catalog.Application.DTOs;

namespace Catalog.Application.Interfaces;

public interface IProductReadRepository
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
}
