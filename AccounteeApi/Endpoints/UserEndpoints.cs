using AccounteeCQRS.Requests.User;
using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGroup("user")
            .MapEndpoints()
            .RequireAuthorization();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetUsers)
            .Produces<PagedList<UserResponse>>();

        group.MapGet("/{userId}", GetUserById)
            .Produces<UserResponse>();

        group.MapPost("/", RegisterUser)
            .Produces<UserResponse>();

        group.MapPut("/", EditUser)
            .Produces<UserResponse>();

        group.MapPut("/{userId}/role/{roleId}", ChangeUserRole)
            .Produces<UserResponse>();
        
        group.MapDelete("/{userId}", DeleteUser)
            .Produces<bool>();

        return group;
    }
    
    private static async Task<IResult> GetUsers(
        IMediator mediator,
        [FromQuery] string? searchValue,
        [AsParameters] OrderFilter orderFilter, 
        [AsParameters] PageFilter pageFilter, 
        CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery(searchValue, orderFilter, pageFilter);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetUserById(
        IMediator mediator,
        int userId, 
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(userId);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> RegisterUser(       
        IMediator mediator,
        [FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> EditUser(        
        IMediator mediator,
        [FromBody] EditUserCommand comand, 
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(comand, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> ChangeUserRole(    
        IMediator mediator,
        int roleId,
        int userId, 
        CancellationToken cancellationToken)
    {
        var command = new ChangeUserRoleCommand(userId, roleId);
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteUser(        
        IMediator mediator,
        int userId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(userId);
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
}