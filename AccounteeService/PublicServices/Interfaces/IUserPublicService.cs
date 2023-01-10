using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Contracts.Requests;

namespace AccounteeService.PublicServices.Interfaces;

public interface IUserPublicService
{
    Task<PagedList<UserDto>> GetUsers(string? searchValue, OrderFilter orderFilter, PageFilter pageFilter, CancellationToken cancellationToken);
    Task<UserDto> GetUserById(int userId, CancellationToken cancellationToken);
    Task<UserDto> RegisterUser(RegistrationRequest request, CancellationToken cancellationToken);
    Task<UserDto> EditUser(int userId, UserDto model, CancellationToken cancellationToken);
    Task<bool> DeleteUser(int userId, CancellationToken cancellationToken);
    Task<bool> ChangeUserRole(int roleId, int userId, CancellationToken cancellationToken);
}