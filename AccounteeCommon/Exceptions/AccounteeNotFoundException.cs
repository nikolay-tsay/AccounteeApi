using Microsoft.AspNetCore.Http;

namespace AccounteeCommon.Exceptions;

public sealed class AccounteeNotFoundException : AccounteeException
{
    public AccounteeNotFoundException(string? message = null) : base(message){ }

    public override int GetStatusCode()
    {
        return StatusCodes.Status404NotFound;
    }
}