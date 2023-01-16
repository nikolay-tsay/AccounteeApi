using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Income;
using AccounteeCQRS.Responses.Income;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Income;

public sealed class AddProductToIncomeHandler : IRequestHandler<AddProductToIncomeCommand, IncomeDetailResponse>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public AddProductToIncomeHandler(
        IIncomeRepository incomeRepository, 
        IProductRepository productRepository,
        ICurrentUserService currentUserService, 
        IMapper mapper)
    {
        _incomeRepository = incomeRepository;
        _productRepository = productRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<IncomeDetailResponse> Handle(AddProductToIncomeCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditOutlay, cancellationToken);

        var income = await _incomeRepository.GetByIdDetail(request.Id, true, false, cancellationToken);
        foreach (var toAdd in request.Products)
        {
            var product = await _productRepository.GetById(toAdd.Id, true, false, cancellationToken);
            await _incomeRepository.AddProductToIncome(income!, product!, toAdd.Amount, false, cancellationToken);
        }

        await _incomeRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<IncomeDetailResponse>(income);
        return mapped;
    }
}