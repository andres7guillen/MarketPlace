namespace Contracts.Events;

public record ProductCreatedIntegrationEvent(Guid Id,string Name,decimal Price);
