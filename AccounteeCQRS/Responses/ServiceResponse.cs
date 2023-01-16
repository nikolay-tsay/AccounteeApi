namespace AccounteeCQRS.Responses;

public record ServiceResponse
{
    public required int Id { get; init; }
    public required int IdCategory { get; init; }
    public required string Name { get; init; }
    public required string CategoryName { get; init; }
    public string? Description { get; init; }
    public required decimal TotalPrice { get; init; }
}