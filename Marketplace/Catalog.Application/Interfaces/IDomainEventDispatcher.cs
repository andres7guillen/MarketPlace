using SharedKernel.Common;

namespace Catalog.Application.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<DomainEvent> domainEvents);
}
