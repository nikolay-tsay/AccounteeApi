using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Income;
using AccounteeCQRS.Responses.Income;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Income;

public sealed class DeleteUserFromIncomeHandler : IRequestHandler<DeleteUserFromIncomeCommand, IncomeDetailResponse>
{
    private readonly AccounteeContext _context;
    private readonly IIncomeRepository _incomeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    
    public DeleteUserFromIncomeHandler(
        IIncomeRepository incomeRepository,
        ICurrentUserService currentUserService,
        IMapper mapper, AccounteeContext context)
    {
        _context = context;
        _incomeRepository = incomeRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<IncomeDetailResponse> Handle(DeleteUserFromIncomeCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(false, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanEditOutlay);
        
        var income = await _incomeRepository.GetByIdDetail(request.Id, true, false, cancellationToken);
        if (income!.UserIncomeList is null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.ExpectedDoesNotExist), nameof(income.UserIncomeList), nameof(IncomeEntity), income.Id));
        }

        foreach (var incomeUser in request.Users)
        {
            var toDelete = await income.UserIncomeList!
                .Where(x => x.IdUser == incomeUser.Id)
                .FirstOrNotFoundAsync(cancellationToken);

            _context.UserIncomes.Remove(toDelete);
        }

        await _incomeRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<IncomeDetailResponse>(income);
        return mapped;
    }
}