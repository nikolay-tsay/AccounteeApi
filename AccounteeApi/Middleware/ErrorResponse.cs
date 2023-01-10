namespace AccounteeApi.Middleware;

public class ErrorResponse
{
    public IEnumerable<string> ErrorMessage { get; set; }

    public ErrorResponse(IEnumerable<string> errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}