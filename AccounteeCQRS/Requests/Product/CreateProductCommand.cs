using AccounteeCQRS.Responses;
using AccounteeDomain.Entities.Enums;
using MediatR;

namespace AccounteeCQRS.Requests.Product;

public record CreateProductCommand : IRequest<ProductResponse>
{
    public required int IdCategory { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required MeasurementUnits AmountUnit { get; init; }
    public required decimal Amount { get; init; }
    public required decimal TotalPrice { get; init; }
}