namespace AccounteeCQRS.Responses;

public record RegisterResponse 
{
    public string? Login { get; init; }
    public required string FirstName { get; init; } 
    public required string LastName { get; init; }
    public required string Email { get; init; }
}