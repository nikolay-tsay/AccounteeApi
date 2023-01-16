using MediatR;

namespace AccounteeCQRS.Requests.User;

public record DeleteUserCommand(int Id) : IRequest<bool>;