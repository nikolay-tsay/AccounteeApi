using Microsoft.AspNetCore.Http;

namespace AccounteeCommon.Exceptions;

public class AccounteeException : Exception
{
    public AccounteeException(string? message = null) : base(message) { }
    
    public virtual int GetStatusCode()
    {
        return StatusCodes.Status500InternalServerError;
    }
}