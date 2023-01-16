using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Category;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using MediatR;

namespace AccounteeCQRS.Handlers.Category;

public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCategoryHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUserService)
    {
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanDeleteCategories, cancellationToken);

        var category = await _categoryRepository.GetById(request.Id, request.Target, true, false, cancellationToken);
        await _categoryRepository.DeleteCategory(category!, true, cancellationToken);

        return true;
    }
}