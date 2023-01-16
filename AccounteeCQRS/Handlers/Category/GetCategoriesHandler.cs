using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Category;
using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Category;

public sealed class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, PagedList<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetCategoriesHandler(IMapper mapper, ICategoryRepository categoryRepository, ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<PagedList<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanReadCategories, cancellationToken);

        var categories = await _categoryRepository
            .GetByTarget(request.Target)
            .ApplySearch(request.SearchValue)
            .FilterOrder(request.OrderFilter)
            .ToPagedList(request.PageFilter, cancellationToken);

        var mapped = _mapper.Map<PagedList<CategoryResponse>>(categories);
        return mapped;
    }
}