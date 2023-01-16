using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.Service;

public record EditServiceCommand : IRequest<ServiceResponse>
{
    public required int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public decimal? TotalPrice { get; init; }
}