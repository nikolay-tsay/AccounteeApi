namespace AccounteeCommon.Exceptions;

public class AccounteeException : Exception
{
    public AccounteeException(string? message = null) : base(message) { }
}