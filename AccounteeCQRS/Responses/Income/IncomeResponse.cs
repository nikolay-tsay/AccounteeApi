namespace AccounteeCQRS.Responses.Income;

public record IncomeResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? ServiceName { get; init; }
    public required string CategoryName { get; init; } 
    public string? Description { get; init; }
    public required DateTime DateTime { get; init; }
    public required DateTime LastEdited { get; init; }
    public required decimal TotalAmount { get; init; }
}