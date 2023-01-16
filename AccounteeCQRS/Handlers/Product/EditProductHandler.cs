using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Product;
using AccounteeCQRS.Responses;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Product;

public sealed class EditProductHandler : IRequestHandler<EditProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public EditProductHandler(IProductRepository productRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _productRepository = productRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<ProductResponse> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditProducts, cancellationToken);
        
        var product = await _productRepository.GetById(request.Id, true, false, cancellationToken);
        
        product!.Name = request.Name ?? product.Name;
        product.Description = request.Description ?? product.Description;
        product.AmountUnit = request.AmountUnit ?? product.AmountUnit;
        product.Amount = request.Amount ?? product.Amount;
        product.TotalPrice = request.TotalPrice ?? product.TotalPrice;

        await _productRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<ProductResponse>(product);
        return mapped;
    }
}