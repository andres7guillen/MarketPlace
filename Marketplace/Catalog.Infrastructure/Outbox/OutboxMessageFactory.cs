using Catalog.Domain.Outbox;
using SharedKernel.Common;
using System.Text.Json;

namespace Catalog.Infrastructure.Outbox;

public static class OutboxMessageFactory
{
    public static OutboxMessage Create(DomainEvent domainEvent)
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = domainEvent.GetType().Name,
            Content = JsonSerializer.Serialize(domainEvent)
        };
    }
}
