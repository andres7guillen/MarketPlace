using SharedKernel.Common;

namespace Catalog.Domain.Events;

public class ProductCreatedDomainEvent : DomainEvent
{
    public Guid Id { get; }
    public string Name { get; }
    public decimal Price { get; }

    public ProductCreatedDomainEvent(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}
