using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

namespace Catalog.Application.Features.Products.CreateProduct;

public class CreateProductHandler
    : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _repo;

    public CreateProductHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Guid>> Handle(
        CreateProductCommand request,
        CancellationToken ct)
    {
        var productResult = Product.Create(request.Name, request.Price);

        if (productResult.IsFailure)
            return Result.Failure<Guid>(productResult.Error);

        var saveResult = await _repo.AddAsync(productResult.Value);

        if (saveResult.IsFailure)
            return Result.Failure<Guid>(saveResult.Error);

        return Result.Success(productResult.Value.Id);
    }
}
