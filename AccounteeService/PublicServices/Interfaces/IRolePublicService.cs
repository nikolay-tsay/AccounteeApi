using AccounteeDomain.Models;
using AccounteeService.Contracts;

namespace AccounteeService.PublicServices.Interfaces;

public interface IRolePublicService
{
    Task<PagedList<RoleDto>> GetRoles(PageFilter filter, CancellationToken cancellationToken);
    Task<RoleDto> GetRoleById(int roleId, CancellationToken cancellationToken);
    Task<RoleDto> CreateRole(RoleDto model, CancellationToken cancellationToken);
    Task<RoleDto> EditRole(int roleId, RoleDto model, CancellationToken cancellationToken);
    Task<bool> DeleteRole(int roleId, CancellationToken cancellationToken);
}