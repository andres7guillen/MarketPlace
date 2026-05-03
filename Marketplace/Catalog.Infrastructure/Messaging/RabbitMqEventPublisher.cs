using Catalog.Application.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Catalog.Infrastructure.Messaging;

public class RabbitMqEventPublisher : IEventPublisher, IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string _exchangeName = "catalog.exchange";

    public RabbitMqEventPublisher()
    {
        _factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("RabbitMQ__Host") ?? "rabbitmq",
            UserName = "guest",
            Password = "guest"
        };
    }

    private async Task EnsureConnectionAsync()
    {
        if (_connection is not null && _channel is not null)
            return;

        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(
            exchange: _exchangeName,
            type: ExchangeType.Topic,
            durable: true
        );
    }

    public async Task PublishAsync<T>(T @event)
    {
        await EnsureConnectionAsync();

        var routingKey = typeof(T).Name;

        var json = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await _channel!.BasicPublishAsync(
            exchange: _exchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body
        );
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.CloseAsync();

        if (_connection is not null)
            await _connection.CloseAsync();
    }
}
