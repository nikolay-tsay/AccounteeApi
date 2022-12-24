using AccounteeDomain.Models;
using AccounteeService.Contracts;

namespace AccounteeService.PublicServices.Interfaces;

public interface IAuthPublicService
{
    Task<string> Login(string login, string password, CancellationToken cancellationToken);
    Task<UserDto> RegisterUser(RegistrationRequest request, CancellationToken cancellationToken);
    Task<bool> ChangePassword(int? userId, string oldPwd, string newPwd, CancellationToken cancellationToken);
}