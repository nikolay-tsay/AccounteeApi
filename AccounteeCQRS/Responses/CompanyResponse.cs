namespace AccounteeCQRS.Responses;

public record CompanyResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required decimal Budget { get; init; }
    public string? PhoneNumber { get; init; }
}