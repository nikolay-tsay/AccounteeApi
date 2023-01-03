using AccounteeApi.Controllers.Base;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Controllers;

public class RoleController : BaseController
{
    private IRolePublicService RolePublicService { get; }

    public RoleController(IRolePublicService rolePublicService)
    {
        RolePublicService = rolePublicService;
    }

    [HttpGet("Role")]
    [ProducesResponseType(typeof(PagedList<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoles([FromQuery] PageFilter filter, CancellationToken cancellationToken)
    {
        var result = await RolePublicService.GetRoles(filter, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("Role/{roleId}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleById(int roleId, CancellationToken cancellationToken)
    {
        var result = await RolePublicService.GetRoleById(roleId, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("Role")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateRole([FromBody] RoleDto model, CancellationToken cancellationToken)
    {
        var result = await RolePublicService.CreateRole(model, cancellationToken);

        return Ok(result);
    }
    
    [HttpPut("Role/{roleId}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditRole(int roleId, [FromBody] RoleDto model, CancellationToken cancellationToken)
    {
        var result = await RolePublicService.EditRole(roleId, model, cancellationToken);

        return Ok(result);
    }
    
    [HttpDelete("Role/{roleId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteRole(int roleId, CancellationToken cancellationToken)
    {
        var result = await RolePublicService.DeleteRole(roleId, cancellationToken);

        return Ok(result);
    }
}