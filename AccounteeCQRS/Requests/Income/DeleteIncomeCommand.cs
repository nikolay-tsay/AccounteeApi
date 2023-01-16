using MediatR;

namespace AccounteeCQRS.Requests.Income;

public record DeleteIncomeCommand(int Id) : IRequest<bool>;