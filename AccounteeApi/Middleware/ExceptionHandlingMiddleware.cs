using AccounteeCommon.Exceptions;

namespace AccounteeApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private RequestDelegate Next { get; }
    private ILogger<ExceptionHandlingMiddleware> Logger { get; }

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        Next = next;
        Logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await Next(httpContext);
        }
        catch (Exception ex)
        {
            HandleException(httpContext, ex);
        }
    }

    private void HandleException(HttpContext httpContext, Exception ex)
    {
        if (httpContext.Response.HasStarted)
        {
            Logger.LogWarning("The response has already started, the http status code middleware will not be executed");
            throw ex;
        }

        Logger.LogError(ex, nameof(ExceptionHandlingMiddleware));

        var exBase = ex.GetBaseException();

        httpContext.Response.Clear();
        httpContext.Response.StatusCode = exBase is AccounteeException exception 
            ? exception.GetStatusCode() 
            : StatusCodes.Status500InternalServerError;
        
        httpContext.Response.ContentType = @"application/json";
    }
}