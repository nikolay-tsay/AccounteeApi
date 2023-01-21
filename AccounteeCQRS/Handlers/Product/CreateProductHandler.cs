using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Product;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Product;

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateProductHandler(IProductRepository productRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _productRepository = productRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(false, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanCreateProducts);

        var existing = await _productRepository.GetByName(request.Name, false, true, cancellationToken);
        if (existing is not null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture,
                nameof(Resources.AlreadyExists), nameof(ProductEntity)));
        }
        
        var newProduct = _mapper.Map<ProductEntity>(request);
        if (newProduct is null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture,
                nameof(Resources.MappingError), nameof(CreateProductCommand), nameof(ProductEntity)));
        }

        newProduct.IdCompany = currentUser.User.IdCompany;
        await _productRepository.AddProduct(newProduct, true, cancellationToken);

        var mapped = _mapper.Map<ProductResponse>(newProduct);
        return mapped;
    }
}