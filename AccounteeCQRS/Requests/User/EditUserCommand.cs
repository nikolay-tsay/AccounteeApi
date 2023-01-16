using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.User;

public record EditUserCommand : IRequest<UserResponse>
{
    public required  int Id { get; init; }
    public string? Login { get; init; }
    public string? FirstName { get; init; } 
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public decimal? IncomePercent { get; init; }
};