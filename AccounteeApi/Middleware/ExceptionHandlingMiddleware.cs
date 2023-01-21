using AccounteeCommon.Exceptions;

namespace AccounteeApi.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleException(httpContext, ex);
        }
    }

    private async Task HandleException(HttpContext httpContext, Exception ex)
    {
        _logger.LogError(ex, nameof(ExceptionHandlingMiddleware));

        var exBase = ex.GetBaseException();
        httpContext.Response.StatusCode = exBase is AccounteeException exception
            ? exception.GetStatusCode()
            : StatusCodes.Status500InternalServerError;

        var response = new ErrorResponse
        {
            StatusCode = httpContext.Response.StatusCode,
            ErrorMessage = exBase.Message
        };
        
        httpContext.Response.ContentType = @"application/json";
        await httpContext.Response.WriteAsync(response.ToString());
    }
}