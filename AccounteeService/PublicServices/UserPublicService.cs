﻿using AccounteeCommon.Exceptions;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Enums;
using AccounteeService.Extensions;
using AccounteeService.PrivateServices.Interfaces;
using AccounteeService.PublicServices.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.PublicServices;

public class UserPublicService : IUserPublicService
{
    private ICurrentUserPrivateService CurrentUserPrivateService { get; }
    private AccounteeContext AccounteeContext { get; }
    private IPasswordHandler PasswordHandler { get; }
    private IMapper Mapper { get; }

    public UserPublicService(ICurrentUserPrivateService currentUserPrivateService, AccounteeContext accounteeContext, IMapper mapper, IPasswordHandler passwordHandler)
    {
        CurrentUserPrivateService = currentUserPrivateService;
        AccounteeContext = accounteeContext;
        Mapper = mapper;
        PasswordHandler = passwordHandler;
    }
    
    public async Task<PagedList<UserDto>> GetUsers(PageFilter filter, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser, UserRights.CanReadUsers);
        
        var users = AccounteeContext.Users
            .AsNoTracking()
            .Where(x => x.IdCompany == currentUser.IdCompany)
            .ToPagedList(filter.PageNum, filter.PageSize);
        
        var mapped = Mapper.Map<PagedList<UserEntity>, PagedList<UserDto>>(users);

        return mapped;
    }

    public async Task<UserDto> GetUserById(int userId, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser, UserRights.CanReadUsers);

        var user = await AccounteeContext.Users
            .AsNoTracking()
            .Where(x => x.IdCompany == currentUser.IdCompany)
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);

        var mapped = Mapper.Map<UserDto>(user);
        return mapped;
    }

    public async Task<UserDto> RegisterUser(RegistrationRequest request, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser, UserRights.CanRegisterUsers);
        
        var exists = await AccounteeContext.Users
            .Where(x => x.IdCompany == currentUser.IdCompany)
            .Where(x => x.Login == request.Login)
            .AnyAsync(cancellationToken);

        if (exists)
        {
            throw new AccounteeException("User already exists");
        }
        
        PasswordHandler.CreateHash(request.Password, out string hash, out string salt);
        var newUser = new UserEntity
        {
            IdCompany = currentUser.IdCompany,
            IdRole = request.IdRole,
            Login = request.Login,
            PasswordHash = hash,
            PasswordSalt = salt,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        AccounteeContext.Add(newUser);
        await AccounteeContext.SaveChangesAsync(cancellationToken);
        
        var mapped = Mapper.Map<UserDto>(newUser);
        return mapped;
    }

    public async Task<UserDto> EditUser(int userId, UserDto model, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser, UserRights.CanEditUsers);

        var user = await AccounteeContext.Users
            .Where(x => x.IdCompany == currentUser.IdCompany)
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);

        user.Login = model.Login ?? user.Login;
        user.FirstName = model.FirstName ?? user.FirstName;
        user.LastName = model.LastName ?? user.LastName;
        user.Email = model.Email ?? user.Email;
        user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;

        await AccounteeContext.SaveChangesAsync(cancellationToken);
        var mapped = Mapper.Map<UserDto>(user);

        return mapped;
    }

    public async Task<bool> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser, UserRights.CanEditUsers);
        
        var user = await AccounteeContext.Users
            .Where(x => x.IdCompany == currentUser.IdCompany)
            .Where(x => x.Id != currentUser.Id)
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);

        AccounteeContext.Users.Remove(user);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ChangeUserRole(int roleId, int userId, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser, UserRights.CanEditUsers);
        CurrentUserPrivateService.CheckUserRights(currentUser, UserRights.CanEditRoles);

        var user = await AccounteeContext.Users
            .Where(x => x.IdCompany == currentUser.IdCompany)
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);

        var role = await AccounteeContext.Roles
            .Where(x => x.IdCompany == currentUser.IdCompany)
            .Where(x => x.Id == roleId)
            .FirstOrNotFound(cancellationToken);

        user.Role = role;

        await AccounteeContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}