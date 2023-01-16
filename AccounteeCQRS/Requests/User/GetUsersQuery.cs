using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using MediatR;

namespace AccounteeCQRS.Requests.User;

public record GetUsersQuery(string? SearchValue, OrderFilter OrderFilter, PageFilter PageFilter) 
    : IRequest<PagedList<UserResponse>>;