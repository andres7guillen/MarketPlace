using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Products.GetAllProducts;

public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;
