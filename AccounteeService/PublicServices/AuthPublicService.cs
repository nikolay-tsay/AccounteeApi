﻿using AccounteeCommon.Exceptions;
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
    private AccounteeContext AccounteeContext { get; }
    private IAuthPrivateService AuthPrivateService { get; }
    private IPasswordHandler PasswordHandler { get; }
    private IMapper Mapper { get; }

    public AuthPublicService(AccounteeContext accounteeContext, IAuthPrivateService authPrivateService, IMapper mapper, IPasswordHandler passwordHandler)
    {
        AccounteeContext = accounteeContext;
        AuthPrivateService = authPrivateService;
        Mapper = mapper;
        PasswordHandler = passwordHandler;
    }
    
    public async Task<string> Login(string login, string password, CancellationToken cancellationToken)
    {
        var user = await AccounteeContext.Users
            .AsNoTracking()
            .Where(x => x.Login == login)
            .FirstOrNotFound(cancellationToken);

        var pwdValid = PasswordHandler.IsValid(password, user.PasswordHash, user.PasswordSalt);
        if (!pwdValid)
        {
            throw new AccounteeUnauthorizedException();
        }

        var token = AuthPrivateService.GetJwtToken(user, cancellationToken);
        return token;
    }

    public async Task<UserDto> RegisterUser(RegistrationRequest request, CancellationToken cancellationToken)
    {
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
            IdCompany = request.IdCompany,
            IdRole = request.IdCompany == null 
                ? 1 
                : request.IdRole,
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

    public async Task<bool> ChangePassword(int userId, string oldPwd, string newPwd, CancellationToken cancellationToken)
    {
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