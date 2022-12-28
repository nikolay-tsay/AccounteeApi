﻿using AccounteeDomain.Models;
using AccounteeService.Contracts;

namespace AccounteeService.PublicServices.Interfaces;

public interface IUserPublicService
{
    Task<PagedList<UserDto>> GetUsers(PageFilter filter, CancellationToken cancellationToken);
    Task<UserDto> GetUserById(int userId, CancellationToken cancellationToken);
    Task<UserDto> RegisterUser(RegistrationRequest request, CancellationToken cancellationToken);
    Task<UserDto> EditUser(int userId, UserDto model, CancellationToken cancellationToken);
    Task<bool> DeleteUser(int userId, CancellationToken cancellationToken);
    Task<bool> ChangeUserRole(int roleId, int userId, CancellationToken cancellationToken);
}