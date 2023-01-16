using MediatR;

namespace AccounteeCQRS.Requests.Auth;

public record LoginQuery(string Login, string Password) : IRequest<string>;