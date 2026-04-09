namespace BusinessLayer.RabbitMQ;

public class RabbitMqSettings
{
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = "DemoVH";
    public string QueueName { get; set; } = "FundooQueue";
}
