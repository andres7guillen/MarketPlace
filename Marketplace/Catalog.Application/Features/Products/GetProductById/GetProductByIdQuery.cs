using Catalog.Application.DTOs;
using CSharpFunctionalExtensions;
using MediatR;

namespace Catalog.Application.Features.Products.GetProductById;

public record GetProductByIdQuery(Guid Id)
    : IRequest<Maybe<ProductDto>>;
