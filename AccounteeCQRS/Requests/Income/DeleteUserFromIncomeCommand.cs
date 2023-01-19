using AccounteeCQRS.Responses.Income;
using AccounteeService.Contracts.Models;
using MediatR;

namespace AccounteeCQRS.Requests.Income;

public record DeleteUserFromIncomeCommand(int Id, IEnumerable<UserToIncomeModel> Users) : IRequest<IncomeDetailResponse>;