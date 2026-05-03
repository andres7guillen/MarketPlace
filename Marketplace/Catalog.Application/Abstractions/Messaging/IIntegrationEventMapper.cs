namespace Catalog.Application.Abstractions.Messaging;

public interface IIntegrationEventMapper
{
    string EventType { get; }

    object Map(string content);
}