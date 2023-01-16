using AccounteeDomain.Entities.Enums;

namespace AccounteeCQRS.Responses;

public record UserResponse
{
    public required int Id { get; init; }
    public required int IdRole { get; init; }
    public required string RoleName { get; init; }
    public required UserLanguages UserLanguage { get; init; }
    public required string Login { get; init; }
    public required string FirstName { get; init; } 
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public string? PhoneNumber { get; init; }
    public decimal? IncomePercent { get; init; }
}