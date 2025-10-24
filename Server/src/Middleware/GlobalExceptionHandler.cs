using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace src.Middleware;

public class GlobalExceptionHandler(
    IProblemDetailsService problemDetails,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Could not process a request on Machine: {MachineName}, TraceId: {TraceId}",
            Environment.MachineName, httpContext.TraceIdentifier);

        (int statusCode, string title) = MapException(exception);

        return await problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Type = exception.GetType().Name,
                Status = statusCode,
                Title = title,
                Detail = exception.Message
            }
        });
    }

    private static (int statusCode, string title) MapException(Exception exception)
    {
        return exception switch
        {
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };
    }
}