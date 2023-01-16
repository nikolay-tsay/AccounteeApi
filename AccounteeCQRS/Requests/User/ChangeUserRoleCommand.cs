using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.User;

public record ChangeUserRoleCommand(int UserId, int RoleId) : IRequest<UserResponse>;