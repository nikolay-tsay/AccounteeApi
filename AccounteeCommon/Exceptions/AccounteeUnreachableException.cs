using Microsoft.AspNetCore.Http;

namespace AccounteeCommon.Exceptions;

public sealed class AccounteeUnreachableException : AccounteeException
{
    public AccounteeUnreachableException(string? message = null) : base(message){ }

    public override int GetStatusCode()
    {
        return StatusCodes.Status500InternalServerError;
    }
}