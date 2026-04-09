namespace BusinessLayer.RabbitMQ;

public interface IRabbitMQProducer
{
    Task SendMessageAsync<T>(T message, CancellationToken cancellationToken = default);
}
