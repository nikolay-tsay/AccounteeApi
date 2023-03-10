using AccounteeCQRS.Requests.Income;
using AccounteeCQRS.Responses.Income;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class IncomeEndpoints
{
    public static void MapIncomeEndpoints(this WebApplication app)
    {
        app.MapGroup("income")
            .MapEndpoints()
            .RequireAuthorization();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("", GetIncomes)
            .Produces<PagedList<IncomeResponse>>();
        
        group.MapGet("/user/{userId}", GetUserIncomes)
            .Produces<IncomeResponse>();
        
        group.MapGet("/{incomeId}", GetIncomeDetails)
            .Produces<IncomeDetailResponse>();

        group.MapPost("/", CreateIncome)
            .Produces<bool>();
        
        group.MapPut("/", EditIncome)
            .Produces<bool>();
        
        group.MapDelete("/{incomeId}", DeleteIncome)
            .Produces<bool>();

        group.MapPut("/{incomeId}/category/{categoryId}", ChangeIncomeCategory)
            .Produces<IncomeDetailResponse>();

        group.MapPost("/{incomeId}/user", AddUserToIncome)
            .Produces<IncomeDetailResponse>();
        
        group.MapDelete("/{incomeId}/user", DeleteUserFromIncome)
            .Produces<IncomeDetailResponse>();

        group.MapPost("/{incomeId}/product", AddProductToIncome)
            .Produces<IncomeDetailResponse>();

        group.MapDelete("/{incomeId}/product", DeleteProductFromIncome)
            .Produces<IncomeDetailResponse>();

        return group;
    }
    
    private static async Task<IResult> GetIncomes(
        IMediator mediator,
        [FromQuery] string? searchValue,
        [AsParameters] OrderFilter orderFilter,
        [AsParameters] PageFilter pageFilter, 
        CancellationToken cancellationToken)
    {
        var query = new GetIncomesQuery(searchValue, orderFilter, pageFilter);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetUserIncomes(
        IMediator mediator,
        int userId, 
        [FromQuery] string? searchValue,
        [AsParameters] OrderFilter orderFilter,
        [AsParameters] PageFilter pageFilter, 
        CancellationToken cancellationToken)
    {
        var query = new GetUserIncomesQuery(userId, searchValue, orderFilter, pageFilter);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetIncomeDetails(
        IMediator mediator,
        int incomeId, 
        CancellationToken cancellationToken)
    {
        var query = new GetIncomeDetailQuery(incomeId);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateIncome(
        IMediator mediator,
        [FromBody] CreateIncomeCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> EditIncome(
        IMediator mediator,
        [FromBody] EditIncomeCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteIncome(
        IMediator mediator,
        int incomeId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteIncomeCommand(incomeId);
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> ChangeIncomeCategory(
        IMediator mediator,
        int incomeId,
        int categoryId,
        CancellationToken cancellationToken)
    {
        var command = new ChangeIncomeCategoryCommand(incomeId, categoryId);
        var result = await mediator.Send(command, cancellationToken);
        
        return Results.Ok(result);
    }

    private static async Task<IResult> AddUserToIncome(
        IMediator mediator,
        int incomeId,
        [FromBody] IEnumerable<UserToIncomeModel> models,
        CancellationToken cancellationToken)
    {
        var command = new AddUserToIncomeCommand(incomeId, models);
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteUserFromIncome(
        IMediator mediator,
        int incomeId,
        [FromBody] IEnumerable<UserToIncomeModel> models,
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserFromIncomeCommand(incomeId, models);
        var result = await mediator.Send(command, cancellationToken);
        
        return Results.Ok(result);
    }

    private static async Task<IResult> AddProductToIncome(
        IMediator mediator,
        int incomeId, 
        [FromBody] IEnumerable<ProductToIncomeModel> models,
        CancellationToken cancellationToken)
    {
        var command = new AddProductToIncomeCommand(incomeId, models);
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteProductFromIncome(
        IMediator mediator,
        int incomeId, 
        [FromBody] IEnumerable<ProductToIncomeModel> models, 
        CancellationToken cancellationToken)
    {
        var command = new DeleteProductFromIncomeCommand(incomeId, models);
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
}