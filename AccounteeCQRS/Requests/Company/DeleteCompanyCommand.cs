using MediatR;

namespace AccounteeCQRS.Requests.Company;

public record DeleteCompanyCommand : IRequest<bool>;