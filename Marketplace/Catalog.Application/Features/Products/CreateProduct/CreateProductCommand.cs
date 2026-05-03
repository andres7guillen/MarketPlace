using CSharpFunctionalExtensions;
using MediatR;

namespace Catalog.Application.Features.Products.CreateProduct;

public record CreateProductCommand(string Name, decimal Price) : IRequest<Result<Guid>>;
