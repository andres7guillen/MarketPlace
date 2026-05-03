using Catalog.Domain.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

namespace Catalog.Application.Features.Products.UpdateProduct;

public class UpdateProductHandler
    : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IProductRepository _repo;

    public UpdateProductHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result> Handle(
        UpdateProductCommand request,
        CancellationToken ct)
    {
        var productMaybe = await _repo.GetByIdAsync(request.Id);

        if (productMaybe.HasNoValue)
            return Result.Failure("Product not found");

        var product = productMaybe.Value;

        var updateResult = product.Update(request.Name, request.Price);

        if (updateResult.IsFailure)
            return updateResult;

        return await _repo.UpdateAsync(product);
    }
}
