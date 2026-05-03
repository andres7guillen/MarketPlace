namespace SharedKernel.Common;

public abstract class BaseEntity
{
    private readonly List<DomainEvent> _domainEvents = new();

    public Guid Id { get; protected set; }

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

    protected void AddDomainEvent(DomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}
