using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Income;
using AccounteeCQRS.Responses.Income;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Income;

public sealed class EditIncomeHandler : IRequestHandler<EditIncomeCommand, IncomeDetailResponse>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public EditIncomeHandler(IIncomeRepository incomeRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _incomeRepository = incomeRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<IncomeDetailResponse> Handle(EditIncomeCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditOutlay, cancellationToken);
        
        var income = await _incomeRepository.GetById(request.Id, true, false, cancellationToken);

        income!.Name = request.Name ?? income.Name;
        income.Description = request.Description ?? income.Description;
        income.DateTime = request.DateTime ?? income.DateTime;
        income.LastEdited = DateTime.UtcNow;
        income.TotalAmount = request.TotalAmount ?? income.TotalAmount;

        await _incomeRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<IncomeDetailResponse>(income);
        return mapped;
    }
}