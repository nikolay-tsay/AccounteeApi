using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.Role;

public record GetRoleByIdQuery(int Id) : IRequest<RoleResponse>;