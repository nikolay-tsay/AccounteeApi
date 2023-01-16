using MediatR;

namespace AccounteeCQRS.Requests.Role;

public record DeleteRoleCommand(int Id) : IRequest<bool>;