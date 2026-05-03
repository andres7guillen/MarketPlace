using Catalog.Application.Interfaces;
using Catalog.Domain.Events;
using Contracts.Events;
using MediatR;

namespace Catalog.Application.DomainEventsHandlers;

public class ProductCreatedDomainEventHandler
    : INotificationHandler<ProductCreatedDomainEvent>
{
    private readonly IEventPublisher _eventPublisher;

    public ProductCreatedDomainEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("HANDLER EJECUTADO");
        var integrationEvent = new ProductCreatedIntegrationEvent(
            notification.Id,
            notification.Name,
            notification.Price
        );

        await _eventPublisher.PublishAsync(integrationEvent);
    }
}
