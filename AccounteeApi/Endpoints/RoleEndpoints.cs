using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        app.MapGet("Role", GetRoles)
            .RequireAuthorization()
            .Produces<PagedList<RoleDto>>();
        
        app.MapGet("Role/{roleId}", GetRoleById)
            .RequireAuthorization()
            .Produces<RoleDto>();

        app.MapPost("Role", CreateRole)
            .RequireAuthorization()
            .Produces<RoleDto>();

        app.MapPut("Role/{roleId}", EditRole)
            .RequireAuthorization()
            .Produces<RoleDto>();

        app.MapDelete("Role/{roleId}", DeleteRole)
            .RequireAuthorization()
            .Produces<bool>();
    }
    
    private static async Task<IResult> GetRoles(
        IRolePublicService service,
        [AsParameters] OrderFilter orderFilter, 
        [AsParameters] PageFilter pageFilter, 
        CancellationToken cancellationToken)
    {
        var result = await service.GetRoles(orderFilter, pageFilter, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetRoleById(
        IRolePublicService service,
        int roleId, 
        CancellationToken cancellationToken)
    {
        var result = await service.GetRoleById(roleId, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateRole(
        IRolePublicService service,
        [FromBody] RoleDto model,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateRole(model, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> EditRole(
        IRolePublicService service,
        int roleId, 
        [FromBody] RoleDto model,
        CancellationToken cancellationToken)
    {
        var result = await service.EditRole(roleId, model, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteRole(       
        IRolePublicService service,
        int roleId, 
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteRole(roleId, cancellationToken);

        return Results.Ok(result);
    }
}