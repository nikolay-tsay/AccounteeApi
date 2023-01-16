using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.User;

public record GetUserByIdQuery(int Id) : IRequest<UserResponse>;