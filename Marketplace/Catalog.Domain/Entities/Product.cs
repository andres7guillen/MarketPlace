using Catalog.Domain.Events;
using CSharpFunctionalExtensions;
using SharedKernel.Common;

namespace Catalog.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    private Product(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public static Result<Product> Create(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Product>("Name is required");

        if (price <= 0)
            return Result.Failure<Product>("Price must be greater than 0");

        var product = new Product(Guid.NewGuid(), name, price);

        product.AddDomainEvent(new ProductCreatedDomainEvent(product.Id, product.Name, product.Price));

        return Result.Success(product);
    }

    public Result Update(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("Name is required");

        if (price <= 0)
            return Result.Failure("Price must be greater than 0");

        Name = name;
        Price = price;

        return Result.Success();
    }
}