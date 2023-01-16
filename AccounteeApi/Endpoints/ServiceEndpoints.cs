using AccounteeCQRS.Requests.Service;
using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class ServiceEndpoints
{
    public static void MapServiceEndpoints(this WebApplication app)
    {
        app.MapGroup("service")
            .MapEndpoints()
            .RequireAuthorization();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetServices)
            .Produces<PagedList<ServiceResponse>>();
        
        group.MapGet("/{serviceId}", GetServiceById)
            .Produces<ServiceResponse>();

        group.MapPost("/", CreateService)
            .Produces<ServiceResponse>();

        group.MapPut("/", EditService)
            .Produces<ServiceResponse>();

        group.MapPut("/{serviceId}/category/{categoryId}", ChangeServiceCategory)
            .Produces<ServiceResponse>();

        group.MapDelete("/{serviceId}", DeleteService)
            .Produces<bool>();

        return group;
    }

    private static async Task<IResult> GetServices(
        IMediator mediator,
        [FromQuery] string? searchValue,
        [AsParameters] OrderFilter orderFilter,
        [AsParameters] PageFilter pageFilter,
        CancellationToken cancellationToken)
    {
        var query = new GetServicesQuery(searchValue, orderFilter, pageFilter);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetServiceById(
        IMediator mediator,
        int serviceId,
        CancellationToken cancellationToken)
    {
        var query = new GetServiceByIdQuery(serviceId);
        var result = await mediator.Send(query, cancellationToken);
        
        return Results.Ok(result);
    }

    private static async Task<IResult> CreateService(
        IMediator mediator,
        [FromBody] CreateServiceCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        
        return Results.Ok(result);
    }

    private static async Task<IResult> EditService(
        IMediator mediator,
        [FromBody] EditServiceCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        
        return Results.Ok(result);
    }

    private static async Task<IResult> ChangeServiceCategory(
        IMediator mediator,
        int serviceId,
        int categoryId,
        CancellationToken cancellationToken)
    {
        var command = new ChangeServiceCategoryCommand(serviceId, categoryId);
        var result = await mediator.Send(command, cancellationToken);
        
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteService(
        IMediator mediator,
        int serviceId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteServiceCommand(serviceId);
        var result = await mediator.Send(command, cancellationToken);
        
        return Results.Ok(result);
    }
}