using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.Company;

public record EditCompanyCommand : IRequest<CompanyResponse>
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public decimal? Budget { get; init; }
    public string? PhoneNumber { get; init; }
};