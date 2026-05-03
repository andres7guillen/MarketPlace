using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using MediatR;

namespace Catalog.Application.Features.Products.GetAllProducts;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductReadRepository _repo;

    public GetAllProductsHandler(IProductReadRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request,CancellationToken ct)
    {
        return await _repo.GetAllAsync();
    }
}
