using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using MediatR;

namespace AccounteeCQRS.Requests.Product;

public record GetProductsQuery(string? SearchValue, OrderFilter OrderFilter, PageFilter PageFilter) 
    : IRequest<PagedList<ProductResponse>>;