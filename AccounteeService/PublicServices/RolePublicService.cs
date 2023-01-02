using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Extensions;
using AccounteeService.PrivateServices.Interfaces;
using AccounteeService.PublicServices.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.PublicServices;

public class RolePublicService : IRolePublicService
{
    private ICurrentUserPrivateService CurrentUserPrivateService { get; }
    private AccounteeContext AccounteeContext { get; }
    private IMapper Mapper { get; }

    public RolePublicService(ICurrentUserPrivateService currentUserPrivateService, AccounteeContext accounteeContext, IMapper mapper)
    {
        CurrentUserPrivateService = currentUserPrivateService;
        AccounteeContext = accounteeContext;
        Mapper = mapper;
    }

    public async Task<PagedList<RoleDto>> GetRoles(PageFilter filter, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadRoles, cancellationToken);

        var roles = await AccounteeContext.Roles
            .AsNoTracking()
            .ToPagedList(filter, cancellationToken);
        
        var mapped = Mapper.Map<PagedList<RoleEntity>, PagedList<RoleDto>>(roles);

        return mapped;
    }

    public async Task<RoleDto> GetRoleById(int roleId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadRoles, cancellationToken);

        var role = await AccounteeContext.Roles
            .AsNoTracking()
            .Where(x => x.Id == roleId)
            .FirstOrNotFound(cancellationToken);

        var mapped = Mapper.Map<RoleDto>(role);
        return mapped;
    }

    public async Task<RoleDto> CreateRole(RoleDto model, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUserPrivateService.GetCurrentUser(cancellationToken);
        CurrentUserPrivateService.CheckUserRights(currentUser, UserRights.CanCreateRoles);

        if (string.IsNullOrWhiteSpace(model.Name))
        {
            throw new AccounteeException();
        }

        var newRole = new RoleEntity
        {
            IdCompany = currentUser.IdCompany,
            Name = model.Name,
            Description = model.Description,
            CanEditCompany = model.CanEditCompany ?? false,
            CanReadUsers = model.CanReadUsers ?? false,
            CanRegisterUsers = model.CanRegisterUsers ?? false,
            CanEditUsers = model.CanEditUsers ?? false,
            CanDeleteUsers = model.CanDeleteUsers ?? false,
            CanReadRoles = model.CanReadRoles ?? false,
            CanCreateRoles = model.CanCreateRoles ?? false,
            CanEditRoles = model.CanEditRoles ?? false,
            CanDeleteRoles = model.CanDeleteRoles ?? false,
            CanReadOutlay = model.CanReadOutlay ?? false,
            CanEditOutlay = model.CanEditOutlay ?? false,
            CanCreateOutlay = model.CanCreateOutlay ?? false,
            CanDeleteOutlay = model.CanDeleteOutlay ?? false
        };

        AccounteeContext.Roles.Add(newRole);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<RoleDto>(newRole);

        return mapped;
    }

    public async Task<RoleDto> EditRole(int roleId, RoleDto model, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditRoles, cancellationToken);

        var role = await AccounteeContext.Roles
            .Where(x => x.Id == roleId)
            .FirstOrNotFound(cancellationToken);

        role.Name = model.Name ?? role.Name;
        role.Description = model.Description ?? role.Description;
        role.IsAdmin = model.IsAdmin ?? role.IsAdmin;
        role.CanEditCompany = model.CanEditCompany ?? role.CanEditCompany;
        role.CanReadUsers = model.CanReadUsers ?? role.CanReadUsers;
        role.CanRegisterUsers = model.CanRegisterUsers ?? role.CanRegisterUsers;
        role.CanEditUsers = model.CanEditUsers ?? role.CanEditUsers;
        role.CanDeleteUsers = model.CanDeleteUsers ?? role.CanDeleteUsers;
        role.CanReadRoles = model.CanReadRoles ?? role.CanReadRoles;
        role.CanCreateRoles = model.CanCreateRoles ?? role.CanCreateRoles;
        role.CanEditRoles = model.CanEditRoles ?? role.CanEditRoles;
        role.CanReadOutlay = model.CanReadOutlay ?? role.CanReadOutlay;
        role.CanCreateOutlay = model.CanCreateOutlay ?? role.CanCreateOutlay;
        role.CanEditOutlay = model.CanEditOutlay ?? role.CanEditOutlay;
        role.CanDeleteOutlay = model.CanDeleteOutlay ?? role.CanDeleteOutlay;

        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<RoleDto>(role);
        return mapped;
    }

    public async Task<bool> DeleteRole(int roleId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanDeleteCompany, cancellationToken);
        
        var role = await AccounteeContext.Roles
            .Where(x => x.Id == roleId)
            .FirstOrNotFound(cancellationToken);

        AccounteeContext.Roles.Remove(role);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}