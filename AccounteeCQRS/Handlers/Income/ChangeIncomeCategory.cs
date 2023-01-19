using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Income;
using AccounteeCQRS.Responses.Income;
using AccounteeDomain.Entities.Enums;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Income;

public sealed class ChangeIncomeCategory : IRequestHandler<ChangeIncomeCategoryCommand, IncomeDetailResponse>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public ChangeIncomeCategory(IIncomeRepository incomeRepository, ICategoryRepository categoryRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _incomeRepository = incomeRepository;
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<IncomeDetailResponse> Handle(ChangeIncomeCategoryCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditOutlay, cancellationToken);

        var income = await _incomeRepository.GetById(request.IncomeId, true, false, cancellationToken);
        var category = await _categoryRepository.GetById(request.CategoryId, CategoryTargets.Income, true, false, cancellationToken);

        income!.IdCategory = category!.Id;

        await _incomeRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<IncomeDetailResponse>(income);
        return mapped;
    }
}