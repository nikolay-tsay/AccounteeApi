using AccounteeCommon.Exceptions;
using AccounteeCommon.HttpContexts;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Extensions;
using AccounteeService.PrivateServices.Interfaces;
using AccounteeService.PublicServices.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.PublicServices;

public class AuthPublicService : IAuthPublicService
{
    private ICurrentUserPrivateService CurrentUserPrivateService { get; }
    private AccounteeContext AccounteeContext { get; }
    private IAuthPrivateService AuthPrivateService { get; }
    private IPasswordHandler PasswordHandler { get; }
    private IMapper Mapper { get; }

    public AuthPublicService(AccounteeContext accounteeContext, IAuthPrivateService authPrivateService, IMapper mapper, IPasswordHandler passwordHandler, ICurrentUserPrivateService currentUserPrivateService)
    {
        AccounteeContext = accounteeContext;
        AuthPrivateService = authPrivateService;
        Mapper = mapper;
        PasswordHandler = passwordHandler;
        CurrentUserPrivateService = currentUserPrivateService;
    }
    
    public async Task<string> Login(string login, string password, CancellationToken cancellationToken)
    {
        GlobalHttpContext.SetIgnoreCompanyFilter(true);
        
        var user = await AccounteeContext.Users
            .AsNoTracking()
            .Where(x => x.Login == login)
            .FirstOrNotFound(cancellationToken);

        var pwdValid = PasswordHandler.IsValid(password, user.PasswordHash, user.PasswordSalt);
        if (!pwdValid)
        {
            throw new AccounteeUnauthorizedException();
        }

        if (user.IdCompany != null)
        {
            GlobalHttpContext.SetCompanyId(user.IdCompany.Value);
        }
        
        var token = AuthPrivateService.GetJwtToken(user, cancellationToken);
        
        GlobalHttpContext.SetIgnoreCompanyFilter(false);
        return token;
    }

    public async Task<UserDto> Register(RegistrationRequest request, CancellationToken cancellationToken)
    {
        GlobalHttpContext.SetIgnoreCompanyFilter(true);
        
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
            IdRole = 1,
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
        
        GlobalHttpContext.SetIgnoreCompanyFilter(false);
        return mapped;
    }

    public async Task<bool> ChangePassword(int? userId, string oldPwd, string newPwd, CancellationToken cancellationToken)
    {
        if (userId == null)
        {
            userId = CurrentUserPrivateService.GetCurrentUserId();
        }

        var user = await AccounteeContext.Users
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);

        var pwdValid = PasswordHandler.IsValid(oldPwd, user.PasswordHash, user.PasswordSalt);
        if (!pwdValid)
        {
            throw new AccounteeUnauthorizedException("Old password is not valid");
        }
        
        PasswordHandler.CreateHash(newPwd, out string hash, out string salt);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;

        await AccounteeContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}