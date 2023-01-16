using AccounteeCQRS.Responses;
using AccounteeDomain.Entities.Enums;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using MediatR;

namespace AccounteeCQRS.Requests.Category;

public record GetCategoriesQuery(string? SearchValue, OrderFilter OrderFilter, PageFilter PageFilter, CategoryTargets Target) 
    : IRequest<PagedList<CategoryResponse>>;