using AccounteeCQRS.Responses;
using AccounteeDomain.Entities.Enums;
using MediatR;

namespace AccounteeCQRS.Requests.Product;

public record EditProductCommand : IRequest<ProductResponse>
{
    public required  int Id { get; init; }
    public string? Name { get; init; }
    public string? CategoryName { get; init; }
    public string? Description { get; init; }
    public MeasurementUnits? AmountUnit { get; init; }
    public decimal? Amount { get; init; }
    public decimal? TotalPrice { get; init; }
};