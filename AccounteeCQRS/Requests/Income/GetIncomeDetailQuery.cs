using AccounteeCQRS.Responses.Income;
using MediatR;

namespace AccounteeCQRS.Requests.Income;

public record GetIncomeDetailQuery(int Id) : IRequest<IncomeDetailResponse>;