using AccounteeCQRS.Responses.Income;
using MediatR;

namespace AccounteeCQRS.Requests.Income;

public record ChangeIncomeCategoryCommand(int IncomeId, int CategoryId) : IRequest<IncomeDetailResponse>;