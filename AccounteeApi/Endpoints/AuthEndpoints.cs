using AccounteeCQRS.Requests.Auth;
using AccounteeCQRS.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapGroup("auth")
            .MapEndpoints();
    }

    private static void MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/login", Login)
            .AllowAnonymous()
            .Produces<string>();

        group.MapPost("/register", Register)
            .AllowAnonymous()
            .Produces<RegisterResponse>();

        group.MapPut("/changePassword", ChangePassword)
            .RequireAuthorization()
            .Produces<bool>();
    }
    
    private static async Task<IResult> Login(
        IMediator mediator,
        [AsParameters] LoginQuery query,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> Register(
        IMediator mediator,
        [FromBody] RegisterCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> ChangePassword(
        IMediator mediator,
        [AsParameters] ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
}