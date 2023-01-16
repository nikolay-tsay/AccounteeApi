using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Income;
using AccounteeCQRS.Responses.Income;
using AccounteeDomain.Entities;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Income;

public sealed class DeleteProductFromIncomeHandler : IRequestHandler<DeleteProductFromIncomeCommand, IncomeDetailResponse>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public DeleteProductFromIncomeHandler(
        IIncomeRepository incomeRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _incomeRepository = incomeRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<IncomeDetailResponse> Handle(DeleteProductFromIncomeCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(false, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanEditOutlay);

        var income = await _incomeRepository.GetByIdDetail(request.Id, true, false, cancellationToken);
        if (income!.IncomeProductList is null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.ExpectedDoesNotExist), 
                new object[] {nameof(income.IncomeProductList), nameof(IncomeEntity), income.Id}));
        }

        foreach (var incomeProduct in request.Products)
        {
            var toDelete = income.IncomeProductList
                .Where(x => x.IdProduct == incomeProduct.Id)
                .FirstOrNotFound();

            await _incomeRepository.DeleteProductFromIncome(income, toDelete, incomeProduct.Amount, false, cancellationToken);
        }

        await _incomeRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<IncomeDetailResponse>(income);
        return mapped;
    }
}