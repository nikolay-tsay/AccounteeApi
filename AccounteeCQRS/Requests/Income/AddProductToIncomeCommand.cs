using AccounteeCQRS.Responses.Income;
using AccounteeService.Contracts.Models;
using MediatR;

namespace AccounteeCQRS.Requests.Income;

public record AddProductToIncomeCommand(int Id, IEnumerable<ProductToIncomeModel> Products) 
    : IRequest<IncomeDetailResponse>;