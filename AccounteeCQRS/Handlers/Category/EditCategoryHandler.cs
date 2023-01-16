using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Category;
using AccounteeCQRS.Responses;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Category;

public sealed class EditCategoryHandler : IRequestHandler<EditCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public EditCategoryHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<CategoryResponse> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditCategories, cancellationToken);

        var category = await _categoryRepository.GetById(request.Id, request.Target, true, false, cancellationToken);

        category!.Name = request.Name ?? category.Name;
        category.Description = request.Description ?? category.Description;

        await _categoryRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<CategoryResponse>(category);
        return mapped;
    }
}