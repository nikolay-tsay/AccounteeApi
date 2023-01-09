using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;

namespace AccounteeService.PublicServices.Interfaces;

public interface IRolePublicService
{
    Task<PagedList<RoleDto>> GetRoles(OrderFilter orderFilter, PageFilter pageFilter, CancellationToken cancellationToken);
    Task<RoleDto> GetRoleById(int roleId, CancellationToken cancellationToken);
    Task<RoleDto> CreateRole(RoleDto model, CancellationToken cancellationToken);
    Task<RoleDto> EditRole(int roleId, RoleDto model, CancellationToken cancellationToken);
    Task<bool> DeleteRole(int roleId, CancellationToken cancellationToken);
}