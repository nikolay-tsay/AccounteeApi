using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using MediatR;

namespace AccounteeCQRS.Requests.Service;

public record GetServicesQuery(string? SearchValue, OrderFilter OrderFilter, PageFilter PageFilter) 
    : IRequest<PagedList<ServiceResponse>>;