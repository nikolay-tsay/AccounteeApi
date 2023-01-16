using AccounteeCQRS.Requests.Category;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities.Enums;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGroup("category")
            .MapEndpoints()
            .RequireAuthorization();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetCategories)
            .Produces<PagedList<CategoryResponse>>();

        group.MapPost("/", CreateCategory)
            .Produces<CategoryResponse>();
        
        group.MapPut("/", EditCategory)
            .Produces<CategoryResponse>();

        group.MapDelete("/{id}", DeleteCategory)
            .Produces<bool>();

        return group;
    }
    
    private static async Task<IResult> GetCategories(
        IMediator mediator,
        [FromQuery] string? searchValue,
        [AsParameters] OrderFilter orderFilter,
        [AsParameters] PageFilter pageFilter, 
        [FromQuery] CategoryTargets target,
        CancellationToken cancellationToken)
    {
        var query = new GetCategoriesQuery(searchValue, orderFilter, pageFilter, target);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateCategory(
        IMediator mediator,
        [FromBody] CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> EditCategory(
        IMediator mediator, 
        [FromBody] EditCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteCategory(
        IMediator mediator,
        int id, 
        [FromQuery] CategoryTargets target, 
        CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(id, target);
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
}