using AccounteeCQRS.Requests.Role;
using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        app.MapGroup("role")
            .MapEndpoints()
            .RequireAuthorization();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetRoles)
            .Produces<PagedList<RoleResponse>>();
        
        group.MapGet("/{roleId}", GetRoleById)
            .Produces<RoleResponse>();

        group.MapPost("/", CreateRole)
            .Produces<RoleResponse>();

        group.MapPut("/", EditRole)
            .Produces<RoleResponse>();

        group.MapDelete("/{roleId}", DeleteRole)
            .Produces<bool>();

        return group;
    }
    
    private static async Task<IResult> GetRoles(
        IMediator mediator,
        [FromQuery] string? searchValue,
        [AsParameters] OrderFilter orderFilter, 
        [AsParameters] PageFilter pageFilter, 
        CancellationToken cancellationToken)
    {
        var query = new GetRolesQuery(searchValue, orderFilter, pageFilter);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetRoleById(
        IMediator mediator,
        int roleId, 
        CancellationToken cancellationToken)
    {
        var query = new GetRoleByIdQuery(roleId);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateRole(
        IMediator mediator,
        [FromBody] CreateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> EditRole(
        IMediator mediator,
        [FromBody] EditRoleCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteRole(
        IMediator mediator,
        int roleId, 
        CancellationToken cancellationToken)
    {
        var query = new DeleteRoleCommand(roleId);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }
}