using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.Company;

public record CreateCompanyCommand : IRequest<CompanyResponse>
{
    public required string Name { get; init; }
    public string? Email { get; init; }
    public decimal? Budget { get; init; }
    public string? PhoneNumber { get; init; }
}