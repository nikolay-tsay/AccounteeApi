using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Product;
using AccounteeCQRS.Responses;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Product;

public sealed class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(IProductRepository productRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _productRepository = productRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanReadProducts, cancellationToken);
        
        var product = await _productRepository.GetById(request.Id, false, false, cancellationToken);

        var mapped = _mapper.Map<ProductResponse>(product);
        return mapped;
    }
}