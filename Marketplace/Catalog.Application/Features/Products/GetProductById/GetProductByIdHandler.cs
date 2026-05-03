using Catalog.Application.DTOs;
using Catalog.Domain.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

namespace Catalog.Application.Features.Products.GetProductById;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Maybe<ProductDto>>
{
    private readonly IProductRepository _repo;

    public GetProductByIdHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<Maybe<ProductDto>> Handle(
        GetProductByIdQuery request,
        CancellationToken ct)
    {
        var productMaybe = await _repo.GetByIdAsync(request.Id);

        if (productMaybe.HasNoValue)
            return Maybe<ProductDto>.None;

        var p = productMaybe.Value;

        return Maybe<ProductDto>.From(
            new ProductDto(p.Id, p.Name, p.Price));
    }
}
