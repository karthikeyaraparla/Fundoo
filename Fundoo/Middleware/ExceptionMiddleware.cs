using System.Net;
using System.Text.Json;
using BusinessLayer.Exceptions;

// This middleware catches unhandled exceptions and returns a JSON error response.
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    // RequestDelegate points to the next middleware in the ASP.NET Core pipeline.
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Invoke is the method ASP.NET Core calls for each incoming HTTP request.
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Pass the request to the next middleware or endpoint.
            await _next(context);
        }
        catch (Exception ex)
        {
            // If anything fails later in the pipeline, build a controlled error response here.
            await HandleException(context, ex);
        }
    }

    // This method converts known exceptions into matching HTTP status codes and JSON output.
    private static async Task HandleException(HttpContext context, Exception ex)
    {
        // Tell the client that the response body will be JSON.
        context.Response.ContentType = "application/json";

        var response = new
        {
            Success = false,
            Message = ex.Message
        };

        // Match custom exceptions to the most suitable HTTP status code.
        switch (ex)
        {
            case UserAlreadyExistsException:
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                break;

            case InvalidCredentialsException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;

            case EmailDeliveryException:
                context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        // JsonSerializer.Serialize turns the response object into the JSON text written to the HTTP response body.
        var json = JsonSerializer.Serialize(response);

        // WriteAsync sends the final JSON error response back to the client.
        await context.Response.WriteAsync(json);
    }
}
