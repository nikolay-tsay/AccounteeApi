using Microsoft.AspNetCore.Http;

namespace AccounteeCommon.Exceptions;

public sealed class AccounteeBadOperationException : AccounteeException
{
    public AccounteeBadOperationException(string? message = null) : base(message) { }
    
    public override int GetStatusCode()
    {
        return StatusCodes.Status400BadRequest;
    }
}