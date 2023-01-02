using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Contracts.Models;
using AccounteeService.Contracts.Requests;
using AccounteeService.PublicServices.Interfaces;

namespace AccounteeService.PublicServices;

public class IncomePublicService : IIncomePublicService
{
    public async Task<PagedList<IncomeDto>> GetIncomes(PageFilter filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedList<IncomeDto>> GetUserIncomes(int? userId, PageFilter filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IncomeDetailModel> GetIncomeDetails(int incomeId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateIncome(CreateIncomeRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IncomeDetailModel> EditIncome(IncomeDto model, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IncomeDetailModel> AddProductToIncome(int incomeId, int productId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IncomeDetailModel> DeleteProductFromIncome(int incomeId, int productId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}