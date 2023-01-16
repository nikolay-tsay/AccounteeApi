using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Product;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities.Enums;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Product;

public sealed class ChangeProductCategoryHandler : IRequestHandler<ChangeProductCategoryCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public ChangeProductCategoryHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<ProductResponse> Handle(ChangeProductCategoryCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditProducts, cancellationToken);
        
        var product = await _productRepository.GetById(request.ProductId, true, false, cancellationToken);
        var category = await _categoryRepository.GetById(request.CategoryId, CategoryTargets.Product, true, false, cancellationToken);

        product!.IdCategory = category!.Id;
        
        await _productRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<ProductResponse>(product);
        return mapped;
    }
}