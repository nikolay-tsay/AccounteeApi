using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Contracts.Requests;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("User", GetUsers)
            .RequireAuthorization()
            .Produces<PagedList<UserDto>>();

        app.MapGet("User/{userId}", GetUserById)
            .RequireAuthorization()
            .Produces<UserDto>();

        app.MapPost("User", RegisterUser)
            .RequireAuthorization()
            .Produces<UserDto>();

        app.MapPut("User/{userId}", EditUser)
            .RequireAuthorization()
            .Produces<UserDto>();

        app.MapDelete("User/{userId}", DeleteUser)
            .RequireAuthorization()
            .Produces<bool>();

        app.MapPut("User/{userId}/Role/{roleId}", ChangeUserRole)
            .RequireAuthorization()
            .Produces<UserDto>();
    }
    
    private static async Task<IResult> GetUsers(
        IUserPublicService service,
        [FromQuery] string? searchValue,
        [AsParameters] OrderFilter orderFilter, 
        [AsParameters] PageFilter pageFilter, 
        CancellationToken cancellationToken)
    {
        var result = await service.GetUsers(searchValue, orderFilter, pageFilter, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetUserById(
        IUserPublicService service,
        int userId, 
        CancellationToken cancellationToken)
    {
        var result = await service.GetUserById(userId, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> RegisterUser(       
        IUserPublicService service,
        [FromBody] RegistrationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.RegisterUser(request, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> EditUser(        
        IUserPublicService service,
        int userId,
        [FromBody] UserDto model, 
        CancellationToken cancellationToken)
    {
        var result = await service.EditUser(userId, model, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteUser(        
        IUserPublicService service,
        int userId,
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteUser(userId, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> ChangeUserRole(    
        IUserPublicService service,
        int roleId,
        int userId, 
        CancellationToken cancellationToken)
    {
        var result = await service.ChangeUserRole(roleId, userId, cancellationToken);

        return Results.Ok(result);
    }
}