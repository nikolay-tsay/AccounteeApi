using AccounteeDomain.Entities.Enums;

namespace AccounteeCQRS.Responses;

public record CategoryResponse
{
    public int Id { get; init; } 
    public int? IdCompany { get; init; }
    public required CategoryTargets Target { get; init; }
    public required string Name { get; init; } 
    public string? Description { get; init; }
}