namespace AccounteeCommon.Exceptions;

public class AccounteeUnauthorizedException : Exception
{
    public AccounteeUnauthorizedException(string? message = null) : base(message) { }
}