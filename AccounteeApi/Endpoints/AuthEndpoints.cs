using AccounteeDomain.Models;
using AccounteeService.Contracts.Requests;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapGet("Auth/Login", Login)
            .AllowAnonymous()
            .Produces<string>();

        app.MapPost("Auth/Register", Register)
            .AllowAnonymous()
            .Produces<UserDto>();

        app.MapPut("Auth/ChangePassword", ChangePassword)
            .RequireAuthorization()
            .Produces<UserDto>();
    }
    
    private static async Task<IResult> Login(
        IAuthPublicService service,
        [FromQuery] string login,
        [FromQuery] string password,
        CancellationToken cancellationToken)
    {
        var result = await service.Login(login, password, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> Register(
        IAuthPublicService service, 
        [FromBody] RegistrationRequest request, 
        CancellationToken cancellationToken)
    {
        var result = await service.Register(request, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> ChangePassword(
        IAuthPublicService service, 
        [FromQuery] int? userId, 
        string oldPwd,
        string newPwd,
        CancellationToken cancellationToken)
    {
        var result = await service.ChangePassword(userId, oldPwd, newPwd, cancellationToken);

        return Results.Ok(result);
    }
}