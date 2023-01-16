using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Income;
using AccounteeCQRS.Responses.Income;
using AccounteeDomain.Entities;
using AccounteeService.Contracts.Models;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Income;

public sealed class CreateIncomeHandler : IRequestHandler<CreateIncomeCommand, IncomeDetailResponse>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateIncomeHandler(
        IIncomeRepository incomeRepository,
        IServiceRepository serviceRepository,
        ICurrentUserService currentUserService,
        IMapper mapper,
        IUserRepository userRepository,
        IProductRepository productRepository)
    {
        _incomeRepository = incomeRepository;
        _serviceRepository = serviceRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }
    
    public async Task<IncomeDetailResponse> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(false, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanEditOutlay);

        var newIncome = new IncomeEntity
        {
            IdCompany = currentUser.User.IdCompany,
            IdCategory = request.IdCategory,
            IdService = request.IdService,
            Name = request.Name,
            Description = request.Description,
            DateTime = request.DateTime,
            LastEdited = DateTime.UtcNow
        };

        await CalcIncomeSource(newIncome, request, cancellationToken);
        await CalcUserIncomes(newIncome, request, currentUser, cancellationToken);

        await _incomeRepository.AddIncome(newIncome, true, cancellationToken);

        var mapped = _mapper.Map<IncomeDetailResponse>(newIncome);
        return mapped;
    }

    private async Task CalcUserIncomes(IncomeEntity newIncome, CreateIncomeCommand request, CurrentUser currentUser,
        CancellationToken cancellationToken)
    {
        var userIncomes = request.UserToIncomeRequests ?? new List<UserToIncomeModel>
        {
            new()
            {
                Id = currentUser.User.Id,
                IncomePercent = currentUser.User.IncomePercent
            }
        };

        decimal globalPercent = 0;
        foreach (var userIncome in userIncomes)
        {
            if (globalPercent > 100)
            {
                throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture,
                    nameof(Resources.PercentMax), globalPercent));
            }

            var user = userIncome.Id == currentUser.User.Id
                ? currentUser.User
                : await _userRepository.GetById(userIncome.Id, true, false, cancellationToken);

            await _incomeRepository.AddUserIncome(newIncome, user!, userIncome.IncomePercent, false, cancellationToken);
            globalPercent += userIncome.IncomePercent ?? user!.IncomePercent!.Value;
        }
    }

    private async Task CalcIncomeSource(IncomeEntity newIncome, CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        if (newIncome.IdService is null && request.ProductToIncomeRequests?.Any() == true)
        {
            foreach (var incomeProduct in request.ProductToIncomeRequests)
            {
                var product = await _productRepository.GetById(incomeProduct.Id, true, false, cancellationToken);
                await _incomeRepository.AddProductToIncome(newIncome, product!, incomeProduct.Amount, false, cancellationToken);
            }
            
            return;
        }

        var service = await _serviceRepository.GetByIdWithProductList(newIncome.IdService!.Value, true, false, cancellationToken);
        await _incomeRepository.CalcServiceIncome(newIncome, service!, false, cancellationToken);
    }
}