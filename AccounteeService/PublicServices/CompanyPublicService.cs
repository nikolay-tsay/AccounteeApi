using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.HttpContexts;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Models;
using AccounteeService.Extensions;
using AccounteeService.PrivateServices.Interfaces;
using AccounteeService.PublicServices.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.PublicServices;

public class CompanyPublicService : ICompanyPublicService
{
    private AccounteeContext AccounteeContext { get; }
    private ICurrentUserPrivateService CurrentUserPrivateService { get; }
    private IMapper Mapper { get; }

    public CompanyPublicService(ICurrentUserPrivateService currentUserPrivateService, AccounteeContext accounteeContext, IMapper mapper)
    {
        CurrentUserPrivateService = currentUserPrivateService;
        AccounteeContext = accounteeContext;
        Mapper = mapper;
    }

    public async Task<CompanyDto> GetCompany(CancellationToken cancellationToken)
    {
        var user = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);
        if (user.Company == null)
        {
            throw new AccounteeNotFoundException();
        }

        var mapped = Mapper.Map<CompanyDto>(user.Company);
        return mapped;
    }

    public async Task<CompanyDto> CreateCompany(CompanyDto model, CancellationToken cancellationToken)
    {
        GlobalHttpContext.SetIgnoreCompanyFilter(true);
        
        var userId = CurrentUserPrivateService.GetCurrentUserId();
        var user = await AccounteeContext.Users
            .Include(x => x.Role)
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);
        
        CurrentUserPrivateService.CheckUserRights(user, UserRights.CanCreateCompany);
        ValidateCreateRequest(model);
        var newCompany = new CompanyEntity
        {
            Name = model.Name!,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Budget = model.Budget ?? 0
        };

        await AccounteeContext.Companies.AddAsync(newCompany, cancellationToken);
        var role = await CreateOwnerRole(newCompany, cancellationToken);
        user.Role = role;
        user.Company = newCompany;
        
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<CompanyDto>(newCompany);

        GlobalHttpContext.SetIgnoreCompanyFilter(false);
        return mapped;
    }

    public async Task<bool> DeleteCompany(CancellationToken cancellationToken)
    {
        var userId = CurrentUserPrivateService.GetCurrentUserId();
        var user = await AccounteeContext.Users
            .Include(x => x.Role)
            .Include(x => x.Company)
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);
        
        CurrentUserPrivateService.CheckUserRights(user, UserRights.CanDeleteCompany);
        if (user.Company == null)
        {
            throw new AccounteeException();
        }

        user.IdRole = 1;
        AccounteeContext.Companies.Remove(user.Company);

        await AccounteeContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<CompanyDto> EditCompany(CompanyDto model, CancellationToken cancellationToken)
    {
        var userId = CurrentUserPrivateService.GetCurrentUserId();
        var user = await AccounteeContext.Users
            .Include(x => x.Role)
            .Include(x => x.Company)
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);
        
        CurrentUserPrivateService.CheckUserRights(user, UserRights.CanEditCompany);
        if (user.Company == null)
        {
            throw new AccounteeException();
        }

        user.Company.Email = model.Email ?? user.Company.Email;
        user.Company.PhoneNumber = model.PhoneNumber ?? user.Company.PhoneNumber;
        user.Company.Name = model.Name ?? user.Company.Name;

        await AccounteeContext.SaveChangesAsync(cancellationToken);
        var mapped = Mapper.Map<CompanyDto>(user.Company);
        
        return mapped;
    }

    public async Task<CompanyDto> EditBudget(decimal value, CancellationToken cancellationToken)
    {
        var userId = CurrentUserPrivateService.GetCurrentUserId();
        var user = await AccounteeContext.Users
            .Include(x => x.Role)
            .Include(x => x.Company)
            .Where(x => x.Id == userId)
            .FirstOrNotFound(cancellationToken);
        
        CurrentUserPrivateService.CheckUserRights(user, UserRights.CanEditCompany);
        if (user.Company == null)
        {
            throw new AccounteeException();
        }
        
        user.Company.Budget = value;
        await AccounteeContext.SaveChangesAsync(cancellationToken);
        var mapped = Mapper.Map<CompanyDto>(user.Company);
        
        return mapped;
    }

    private async Task<RoleEntity> CreateOwnerRole(CompanyEntity company, CancellationToken cancellationToken)
    {
        var newRole = new RoleEntity
        {
            Company = company,
            Name = "Owner",
            IsAdmin = true
        };

        await AccounteeContext.Roles.AddAsync(newRole, cancellationToken);
        return newRole;
    }

    private void ValidateCreateRequest(CompanyDto model)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            throw new AccounteeException("Company must have a name");
        }
    }
}