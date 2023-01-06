using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Contracts.Models;
using AccounteeService.Contracts.Requests;

namespace AccounteeService.PublicServices.Interfaces;

public interface IIncomePublicService
{
    Task<PagedList<IncomeDto>> GetIncomes(PageFilter filter, CancellationToken cancellationToken);
    Task<PagedList<IncomeDto>> GetUserIncomes(int? userId, PageFilter filter, CancellationToken cancellationToken);
    Task<IncomeDetailModel> GetIncomeDetails(int incomeId, CancellationToken cancellationToken);
    Task<bool> CreateIncome(CreateIncomeRequest request, CancellationToken cancellationToken);
    Task<IncomeDetailModel> EditIncome(IncomeDto model, int incomeId, CancellationToken cancellationToken);
    Task<bool> DeleteIncome(int incomeId, CancellationToken cancellationToken);
    Task<IncomeDetailModel> AddProductToIncome(int incomeId, IEnumerable<ProductToIncomeRequest> requests, CancellationToken cancellationToken);
    Task<IncomeDetailModel> DeleteProductFromIncome(int incomeId, IEnumerable<ProductToIncomeRequest> requests, CancellationToken cancellationToken);
}