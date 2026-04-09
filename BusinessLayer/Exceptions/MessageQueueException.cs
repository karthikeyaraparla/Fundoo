namespace BusinessLayer.Exceptions;

public class MessageQueueException : Exception
{
    public MessageQueueException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
