using AccounteeCQRS.Responses.Income;
using MediatR;

namespace AccounteeCQRS.Requests.Income;

public record EditIncomeCommand : IRequest<IncomeDetailResponse>
{
    public required int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? DateTime { get; init; }
    public decimal? TotalAmount { get; init; }
};