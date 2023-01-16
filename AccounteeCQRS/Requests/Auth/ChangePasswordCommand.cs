using MediatR;

namespace AccounteeCQRS.Requests.Auth;

public record ChangePasswordCommand(int? UserId, string OldPwd, string NewPwd) : IRequest<bool>;