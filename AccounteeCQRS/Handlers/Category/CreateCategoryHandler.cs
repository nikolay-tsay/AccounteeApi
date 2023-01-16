using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Category;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Category;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(IMapper mapper, ICurrentUserService currentUserService, ICategoryRepository categoryRepository)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(false, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanCreateCategories);

        var existing = await _categoryRepository.GetByName(request.Name, request.Target, false, true, cancellationToken);
        if (existing is not null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture,
                nameof(Resources.AlreadyExists), nameof(CategoryEntity)));
        }

        var newCategory = _mapper.Map<CategoryEntity>(request);
        if (newCategory is null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.MappingError), new object[] {nameof(CreateCategoryCommand), nameof(CategoryEntity)}));
        }

        newCategory.IdCompany = currentUser.User.IdCompany;
        await _categoryRepository.AddCategory(newCategory, true, cancellationToken);

        var mapped = _mapper.Map<CategoryResponse>(newCategory);
        return mapped;
    }
}