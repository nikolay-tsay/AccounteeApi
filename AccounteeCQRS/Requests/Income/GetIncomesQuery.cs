using AccounteeCQRS.Responses.Income;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using MediatR;

namespace AccounteeCQRS.Requests.Income;

public record GetIncomesQuery(string? SearchValue, OrderFilter OrderFilter, PageFilter PageFilter) 
    : IRequest<PagedList<IncomeResponse>>;