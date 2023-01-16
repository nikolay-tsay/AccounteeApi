using AccounteeCQRS.Responses.Income;
using AccounteeService.Contracts.Models;
using MediatR;

namespace AccounteeCQRS.Requests.Income;

public record CreateIncomeCommand : IRequest<IncomeDetailResponse>
{
    public int IdCategory { get; init; }
    public int? IdService { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public DateTime DateTime { get; init; }
    public decimal TotalAmount { get; init; }

    public IEnumerable<ProductToIncomeModel>? ProductToIncomeRequests { get; init; }
    public IEnumerable<UserToIncomeModel>? UserToIncomeRequests { get; init; }
};