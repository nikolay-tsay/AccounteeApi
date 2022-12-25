using Microsoft.AspNetCore.Http;

namespace AccounteeCommon.Exceptions;

public class AccounteeUnauthorizedException : AccounteeException
{
    public AccounteeUnauthorizedException(string? message = null) : base(message) { }

    public override int GetStatusCode()
    {
        return StatusCodes.Status401Unauthorized;
    }
}