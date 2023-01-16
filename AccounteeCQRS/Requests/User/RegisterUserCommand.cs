using System.Text.Json.Serialization;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities.Enums;
using MediatR;

namespace AccounteeCQRS.Requests.User;

public record RegisterUserCommand : IRequest<UserResponse>
{
    public int IdRole { get; init; }
    public required string Login { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required UserLanguages Language { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string? PhoneNumber { get; init; }
    public decimal? IncomePercent { get; init; }
};