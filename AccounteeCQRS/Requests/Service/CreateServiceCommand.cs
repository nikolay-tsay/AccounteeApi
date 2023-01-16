using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.Service;

public record CreateServiceCommand : IRequest<ServiceResponse>
{
    public required int IdCategory { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal TotalPrice { get; set; }
}