using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Income;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using MediatR;

namespace AccounteeCQRS.Handlers.Income;

public sealed class DeleteIncomeHandler : IRequestHandler<DeleteIncomeCommand, bool>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteIncomeHandler(IIncomeRepository incomeRepository, ICurrentUserService currentUserService)
    {
        _incomeRepository = incomeRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<bool> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanDeleteOutlay, cancellationToken);
        
        var income = await _incomeRepository.GetById(request.Id, true, false, cancellationToken);
        await _incomeRepository.DeleteIncome(income!, true, cancellationToken);

        return true;
    }
}