using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Income;
using AccounteeCQRS.Responses.Income;
using AccounteeDomain.Entities;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Income;

public sealed class AddUserToIncomeHandler : IRequestHandler<AddUserToIncomeCommand, IncomeDetailResponse>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public AddUserToIncomeHandler(IIncomeRepository incomeRepository, IUserRepository userRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _incomeRepository = incomeRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<IncomeDetailResponse> Handle(AddUserToIncomeCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(false, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanEditOutlay);
        
        var income = await _incomeRepository.GetByIdDetail(request.Id, true, false, cancellationToken);

        var globalPercent = income!.UserIncomeList!
            .Sum(x => x.User.IncomePercent!.Value);

        foreach (var toAdd in request.Users)
        {
            var user = await _userRepository.GetById(toAdd.Id, true, false, cancellationToken);
            if (user!.IncomePercent is null || toAdd.IncomePercent is not null)
            {
                user.IncomePercent = toAdd.IncomePercent;
            }
            
            if (100 - globalPercent < user.IncomePercent)
            {
                throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture,
                    nameof(Resources.PercentMax), globalPercent));
            }
            
            var currentIncomeUser = income.UserIncomeList!
                .FirstOrDefault(x => x.IdUser == user.Id);

            if (currentIncomeUser != null)
            {
                throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                    nameof(Resources.AlreadyExists), nameof(income.UserIncomeList), nameof(IncomeEntity), income.Id));
            }
            
            await _incomeRepository.AddUserIncome(income, user, null, false, cancellationToken);
        }
        
        await _incomeRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<IncomeDetailResponse>(income);
        return mapped;
    }
}