using Catalog.Application.Abstractions.Messaging;
using Catalog.Application.Interfaces;
using Catalog.Domain.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Outbox;

public class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IEventPublisher _eventPublisher;

    public OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        IEventPublisher eventPublisher)
    {
        _scopeFactory = scopeFactory;
        _eventPublisher = eventPublisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var outboxCollection = scope.ServiceProvider.GetRequiredService<IMongoCollection<OutboxMessage>>();
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
            var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();
            var mappers = scope.ServiceProvider.GetRequiredService<IEnumerable<IIntegrationEventMapper>>();


            var messages = await outboxCollection
           .Find(x => x.ProcessedOnUtc == null)
           .Limit(20)
           .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                    var mapper = mappers.FirstOrDefault(x => x.EventType == message.Type);
                    if (mapper is null)
                        continue;

                    var integrationEvent = mapper.Map(message.Content);

                    await _eventPublisher.PublishAsync(integrationEvent);

                    message.ProcessedOnUtc = DateTime.UtcNow;

                    await outboxCollection.ReplaceOneAsync(
                        x => x.Id == message.Id,
                        message
                    );
                }
                catch
                {
                    // todo: DLQ
                }
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}
