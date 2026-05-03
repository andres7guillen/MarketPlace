namespace Catalog.Infrastructure.Messaging;

public class RabbitMqSettings
{
    public string HostName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ExchangeName { get; set; } = "catalog.exchange";
}
