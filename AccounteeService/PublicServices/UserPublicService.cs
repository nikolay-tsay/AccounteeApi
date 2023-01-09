using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Contracts.Requests;
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
    
    public async Task<PagedList<UserDto>> GetUsers(OrderFilter orderFilter, PageFilter pageFilter, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadUsers, cancellationToken);
        
        var users = await AccounteeContext.Users
            .AsNoTracking()
            .FilterOrder(orderFilter)
            .ToPagedList(pageFilter, cancellationToken);
        
        var mapped = Mapper.Map<PagedList<UserEntity>, PagedList<UserDto>>(users);

        return mapped;
    }

    public async Task<UserDto> GetUserById(int userId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadUsers, cancellationToken);

        var user = await AccounteeContext.Users
            .AsNoTracking()
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
            PhoneNumber = request.PhoneNumber,
            IncomePercent = request.IncomePercent
        };

        AccounteeContext.Add(newUser);
        await AccounteeContext.SaveChangesAsync(cancellationToken);
        
        var mapped = Mapper.Map<UserDto>(newUser);
        return mapped;
    }

    public async Task<UserDto> EditUser(int userId, UserDto model, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditUsers, cancellationToken);

        var user = await AccounteeContext.Users
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);

        user.Login = model.Login ?? user.Login;
        user.FirstName = model.FirstName ?? user.FirstName;
        user.LastName = model.LastName ?? user.LastName;
        user.Email = model.Email ?? user.Email;
        user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
        user.IncomePercent = model.IncomePercent ?? user.IncomePercent;

        await AccounteeContext.SaveChangesAsync(cancellationToken);
        var mapped = Mapper.Map<UserDto>(user);

        return mapped;
    }

    public async Task<bool> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser, UserRights.CanDeleteUsers);
        
        var user = await AccounteeContext.Users
            .Where(x => x.Id != currentUser.Id)
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);

        AccounteeContext.Users.Remove(user);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ChangeUserRole(int roleId, int userId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditUsers, cancellationToken);
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditRoles, cancellationToken);

        var user = await AccounteeContext.Users
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);

        var role = await AccounteeContext.Roles
            .Where(x => x.Id == roleId)
            .FirstOrNotFound(cancellationToken);

        user.Role = role;

        await AccounteeContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}