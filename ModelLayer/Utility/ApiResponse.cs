namespace ModelLayer.Utility;
using System.Collections.Generic;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string[] Errors { get; set; }

    public ApiResponse(bool success, string message, T data)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public ApiResponse(bool success, string message, params string[] errors)
    {
        Success = success;
        Message = message;
        Errors = errors;
    }

    public ApiResponse() { }
}
