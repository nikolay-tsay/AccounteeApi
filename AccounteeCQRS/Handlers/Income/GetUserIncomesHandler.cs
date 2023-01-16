using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Income;
using AccounteeCQRS.Responses.Income;
using AccounteeService.Contracts;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Income;

public sealed class GetUserIncomesHandler : IRequestHandler<GetUserIncomesQuery, PagedList<IncomeResponse>>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetUserIncomesHandler(IIncomeRepository incomeRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _incomeRepository = incomeRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<PagedList<IncomeResponse>> Handle(GetUserIncomesQuery request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanReadOutlay, cancellationToken);
        
        var userIncomes = await _incomeRepository
            .QueryByUser(request.UserId, false)
            .ApplySearch(request.SearchValue)
            .FilterOrder(request.OrderFilter)
            .ToPagedList(request.PageFilter, cancellationToken);

        var mapped = _mapper.Map<PagedList<IncomeResponse>>(userIncomes);
        return mapped;
    }
}