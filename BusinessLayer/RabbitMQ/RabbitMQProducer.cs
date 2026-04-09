using System.Text;
using System.Text.Json;
using BusinessLayer.Exceptions;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace BusinessLayer.RabbitMQ;

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly RabbitMqSettings _settings;

    public RabbitMQProducer(IConfiguration configuration)
    {
        _settings = new RabbitMqSettings
        {
            HostName = configuration["RabbitMQ:HostName"] ?? string.Empty,
            UserName = configuration["RabbitMQ:UserName"] ?? string.Empty,
            Password = configuration["RabbitMQ:Password"] ?? string.Empty,
            VirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/",
            QueueName = configuration["RabbitMQ:QueueName"] ?? "FundooQueue",
            Port = int.TryParse(configuration["RabbitMQ:Port"], out var port) ? port : 5672
        };
    }

    public async Task SendMessageAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        ValidateSettings();

        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };

        try
        {
            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken
            );

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: _settings.QueueName,
                body: body,
                cancellationToken: cancellationToken
            );
        }
        catch (BrokerUnreachableException ex)
        {
            throw new MessageQueueException(
                $"RabbitMQ connection failed. Verify broker settings and ensure RabbitMQ is running at {_settings.HostName}:{_settings.Port}.",
                ex);
        }
    }

    private void ValidateSettings()
    {
        if (string.IsNullOrWhiteSpace(_settings.HostName))
            throw new InvalidOperationException("RabbitMQ HostName is missing.");

        if (_settings.Port <= 0)
            throw new InvalidOperationException("RabbitMQ Port must be greater than 0.");

        if (string.IsNullOrWhiteSpace(_settings.UserName))
            throw new InvalidOperationException("RabbitMQ UserName is missing.");

        if (string.IsNullOrWhiteSpace(_settings.Password))
            throw new InvalidOperationException("RabbitMQ Password is missing.");

        if (string.IsNullOrWhiteSpace(_settings.QueueName))
            throw new InvalidOperationException("RabbitMQ QueueName is missing.");
    }
}
