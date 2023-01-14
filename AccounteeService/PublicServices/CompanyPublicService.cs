using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.HttpContexts;
using AccounteeCommon.Resources;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Models;
using AccounteeService.PrivateServices.Interfaces;
using AccounteeService.PublicServices.Interfaces;
using AutoMapper;

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
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(false, cancellationToken);
        if (currentUser.User.Company == null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.UserNoCompany), currentUser.User.Login));
        }

        var mapped = Mapper.Map<CompanyDto>(currentUser.User.Company);
        return mapped;
    }

    public async Task<CompanyDto> CreateCompany(CompanyDto model, CancellationToken cancellationToken)
    {
        GlobalHttpContext.SetIgnoreCompanyFilter(true);

        var currentUser = await CurrentUserPrivateService.GetCurrentUser(true, cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser.User, UserRights.CanCreateCompany);

        var newCompany = new CompanyEntity
        {
            Name = model.Name!,
            Email = model.Email ?? currentUser.User.Email,
            PhoneNumber = model.PhoneNumber,
            Budget = model.Budget ?? 0
        };

        await AccounteeContext.Companies.AddAsync(newCompany, cancellationToken);
        var role = await CreateOwnerRole(newCompany, cancellationToken);
        currentUser.User.Role = role;
        currentUser.User.Company = newCompany;
        
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<CompanyDto>(newCompany);

        GlobalHttpContext.SetCompanyId(newCompany.Id);
        GlobalHttpContext.SetIgnoreCompanyFilter(false);
        return mapped;
    }

    public async Task<bool> DeleteCompany(CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(true, cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser.User, UserRights.CanDeleteCompany);
        
        if (currentUser.User.Company == null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.UserNoCompany), currentUser.User.Login));
        }

        currentUser.User.IdRole = 1;
        AccounteeContext.Companies.Remove(currentUser.User.Company);

        await AccounteeContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<CompanyDto> EditCompany(CompanyDto model, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(true, cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser.User, UserRights.CanEditCompany);
        
        if (currentUser.User.Company == null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.UserNoCompany), currentUser.User.Login));
        }

        currentUser.User.Company.Email = model.Email ?? currentUser.User.Company.Email;
        currentUser.User.Company.PhoneNumber = model.PhoneNumber ?? currentUser.User.Company.PhoneNumber;
        currentUser.User.Company.Name = model.Name ?? currentUser.User.Company.Name;

        await AccounteeContext.SaveChangesAsync(cancellationToken);
        var mapped = Mapper.Map<CompanyDto>(currentUser.User.Company);
        
        return mapped;
    }

    public async Task<CompanyDto> EditBudget(decimal value, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(true, cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser.User, UserRights.CanEditCompany);
        
        if (currentUser.User.Company == null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.UserNoCompany), currentUser.User.Login));
        }
        
        currentUser.User.Company.Budget = value;
        await AccounteeContext.SaveChangesAsync(cancellationToken);
        var mapped = Mapper.Map<CompanyDto>(currentUser.User.Company);
        
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
}