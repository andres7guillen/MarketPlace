using CSharpFunctionalExtensions;
using MediatR;

namespace Catalog.Application.Features.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id,string Name,decimal Price) : IRequest<Result>;
