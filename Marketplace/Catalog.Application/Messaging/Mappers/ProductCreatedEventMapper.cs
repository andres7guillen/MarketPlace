using Catalog.Application.Abstractions.Messaging;
using Catalog.Domain.Events;
using Contracts.Events;
using System.Text.Json;

namespace Catalog.Application.Messaging.Mappers;

public class ProductCreatedEventMapper : IIntegrationEventMapper
{
    public string EventType => nameof(ProductCreatedDomainEvent);

    public object Map(string content)
    {
        var domainEvent = JsonSerializer.Deserialize<ProductCreatedDomainEvent>(content);

        if (domainEvent is null)
            throw new Exception("Error deserializing ProductCreatedDomainEvent");

        return new ProductCreatedIntegrationEvent(
            domainEvent.Id,
            domainEvent.Name,
            domainEvent.Price
        );
    }
}