using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.Company;

public record GetCompanyQuery : IRequest<CompanyResponse>;