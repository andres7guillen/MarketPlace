using Contracts.Events;
using Inventory.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Inventory.Infrastructure.Messaging.Consumers;

public class ProductCreatedConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private IConnection _connection;
    private IChannel _channel;

    public ProductCreatedConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("RabbitMQ__Host") ?? "rabbitmq"
        };

        var retries = 5;

        while (retries > 0 && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                Console.WriteLine("🔌 Conectando a RabbitMQ...");

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync();

                Console.WriteLine("✅ Conectado a RabbitMQ");
                break;
            }
            catch (Exception ex)
            {
                retries--;
                Console.WriteLine($"❌ Error conectando a RabbitMQ: {ex.Message}");

                if (retries == 0)
                {
                    Console.WriteLine("💥 No se pudo conectar a RabbitMQ");
                    throw;
                }

                Console.WriteLine("🔁 Reintentando en 5 segundos...");
                await Task.Delay(5000, stoppingToken);
            }
        }

        await _channel.ExchangeDeclareAsync(
            exchange: "catalog.exchange",
            type: ExchangeType.Topic,
            durable: true);

        await _channel.QueueDeclareAsync(
            queue: "inventory.product-created",
            durable: true,
            exclusive: false,
            autoDelete: false);

        await _channel.QueueBindAsync(
            queue: "inventory.product-created",
            exchange: "catalog.exchange",
            routingKey: "ProductCreatedIntegrationEvent");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                Console.WriteLine("📩 Evento recibido");

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var integrationEvent = JsonSerializer.Deserialize<ProductCreatedIntegrationEvent>(message);

                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IInventoryService>();

                await service.CreateStockAsync(integrationEvent.Id);

                Console.WriteLine("✅ Stock creado");

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error procesando mensaje: {ex.Message}");

                await _channel.BasicNackAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false,
                    requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: "inventory.product-created",
            autoAck: false,
            consumer: consumer);

        Console.WriteLine("Escuchando eventos...");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_channel?.IsOpen == true)
                await _channel.CloseAsync();

            if (_connection?.IsOpen == true)
                await _connection.CloseAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cerrando RabbitMQ: {ex.Message}");
        }

        await base.StopAsync(cancellationToken);
    }
}