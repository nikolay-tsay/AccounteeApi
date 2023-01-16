using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using MediatR;

namespace AccounteeCQRS.Requests.Role;

public record GetRolesQuery(string? SearchValue, OrderFilter OrderFilter, PageFilter PageFilter)
    : IRequest<PagedList<RoleResponse>>;