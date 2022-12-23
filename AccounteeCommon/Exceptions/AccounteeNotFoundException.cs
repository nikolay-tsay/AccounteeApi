namespace AccounteeCommon.Exceptions;

public class AccounteeNotFoundException : Exception
{
    public AccounteeNotFoundException(string? message = null) : base(message){ }
}