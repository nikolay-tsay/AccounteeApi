using AccounteeCQRS.Responses.Income;
using AccounteeService.Contracts.Models;
using MediatR;

namespace AccounteeCQRS.Requests.Income;

public record DeleteProductFromIncomeCommand(int Id, IEnumerable<ProductToIncomeModel> Products) 
    : IRequest<IncomeDetailResponse>;