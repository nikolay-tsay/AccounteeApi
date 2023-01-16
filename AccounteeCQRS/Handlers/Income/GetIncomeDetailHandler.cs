using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Income;
using AccounteeCQRS.Responses.Income;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Income;

public sealed class GetIncomeDetailHandler : IRequestHandler<GetIncomeDetailQuery, IncomeDetailResponse>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetIncomeDetailHandler(IIncomeRepository incomeRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _incomeRepository = incomeRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<IncomeDetailResponse> Handle(GetIncomeDetailQuery request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanReadOutlay, cancellationToken);
        
        var income = await _incomeRepository.GetByIdDetail(request.Id, false, false, cancellationToken);

        var mapped = _mapper.Map<IncomeDetailResponse>(income);
        return mapped;
    }
}