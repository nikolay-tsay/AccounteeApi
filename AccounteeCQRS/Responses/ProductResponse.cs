using AccounteeDomain.Entities.Enums;

namespace AccounteeCQRS.Responses;

public record ProductResponse
{
    public int Id { get; init; }
    public int IdCategory { get; init; }
    public required string Name { get; init; }
    public required string CategoryName { get; init; }
    public string? Description { get; init; }
    public required MeasurementUnits AmountUnit { get; init; }
    public required decimal Amount { get; init; }
    public required decimal TotalPrice { get; init; }
}