using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Product;
using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccounteeCQRS.Handlers.Product;

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, PagedList<ProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mappped;

    public GetProductsHandler(IProductRepository productRepository, ICurrentUserService currentUserService, IMapper mappped)
    {
        _productRepository = productRepository;
        _currentUserService = currentUserService;
        _mappped = mappped;
    }

    public async Task<PagedList<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanReadProducts, cancellationToken);
        
        var product = await _productRepository
            .QueryAll(false)
            .Include(x => x.ProductCategory)
            .ApplySearch(request.SearchValue)
            .FilterOrder(request.OrderFilter)
            .ToPagedList(request.PageFilter, cancellationToken);

        var mapped = _mappped.Map<PagedList<ProductResponse>>(product);
        return mapped;
    }
}