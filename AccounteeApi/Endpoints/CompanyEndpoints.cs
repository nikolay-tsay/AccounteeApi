using AccounteeCQRS.Requests.Company;
using AccounteeCQRS.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this WebApplication app)
    {
        app.MapGroup("company")
            .MapEndpoints()
            .RequireAuthorization();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetCompany)
            .Produces<CompanyResponse>();

        group.MapPost("/", CreateCompany)
            .Produces<CompanyResponse>();

        group.MapDelete("/", DeleteCompany)
            .Produces<bool>();
        
        group.MapPut("/", EditCompany)
            .Produces<CompanyResponse>();

        return group;
    }
    
    private static async Task<IResult> GetCompany(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyQuery();
        var res = await mediator.Send(query, cancellationToken);

        return Results.Ok(res);
    }
    
    private static async Task<IResult> CreateCompany(
        IMediator mediator,
        [FromBody] CreateCompanyCommand command, 
        CancellationToken cancellationToken)
    {
        var res = await mediator.Send(command, cancellationToken);

        return Results.Ok(res);
    }
    
    private static async Task<IResult> DeleteCompany(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCompanyCommand();
        var res = await mediator.Send(command, cancellationToken);

        return Results.Ok(res);
    }
    
    private static async Task<IResult> EditCompany(
        IMediator mediator,
        [FromBody] EditCompanyCommand command,
        CancellationToken cancellationToken)
    {
        var res = await mediator.Send(command, cancellationToken);

        return Results.Ok(res);
    }
}