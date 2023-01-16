using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Product;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using MediatR;

namespace AccounteeCQRS.Handlers.Product;

public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteProductHandler(IProductRepository productRepository, ICurrentUserService currentUserService)
    {
        _productRepository = productRepository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanDeleteProducts, cancellationToken);
        
        var product = await _productRepository.GetById(request.Id, true, false, cancellationToken);
        await _productRepository.DeleteProduct(product!, true, cancellationToken);

        return true;
    }
}