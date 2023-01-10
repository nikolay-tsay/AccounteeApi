using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Contracts.Models;
using AccounteeService.Contracts.Requests;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class IncomeEndpoints
{
    public static void MapIncomeEndpoints(this WebApplication app)
    {
        app.MapGet("Income", GetIncomes)
            .RequireAuthorization()
            .Produces<PagedList<IncomeDto>>();
        
        app.MapGet("Income/User/{userId}", GetUserIncomes)
            .RequireAuthorization()
            .Produces<IncomeDto>();
        
        app.MapGet("Income/{incomeId}", GetIncomeDetails)
            .RequireAuthorization()
            .Produces<IncomeDetailModel>();

        app.MapPost("Income", CreateIncome)
            .RequireAuthorization()
            .Produces<bool>();
        
        app.MapPut("Income/{incomeId}", EditIncome)
            .RequireAuthorization()
            .Produces<bool>();
        
        app.MapDelete("Income/{incomeId}", DeleteIncome)
            .RequireAuthorization()
            .Produces<bool>();

        app.MapPost("Income/{incomeId}/Product", AddProductToIncome)
            .RequireAuthorization()
            .Produces<IncomeDetailModel>();

        app.MapDelete("Income/{incomeId}/Product", DeleteProductFromIncome)
            .RequireAuthorization()
            .Produces<IncomeDetailModel>();
    }
    
    private static async Task<IResult> GetIncomes(
        IIncomePublicService service,
        [AsParameters] OrderFilter orderFilter,
        [AsParameters] PageFilter pageFilter, 
        CancellationToken cancellationToken)
    {
        var result = await service.GetIncomes(orderFilter, pageFilter, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetUserIncomes(
        IIncomePublicService service,
        int? userId, 
        [AsParameters] OrderFilter orderFilter,
        [AsParameters] PageFilter pageFilter, 
        CancellationToken cancellationToken)
    {
        var result = await service.GetUserIncomes(userId, orderFilter, pageFilter, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetIncomeDetails(
        IIncomePublicService service,
        int incomeId, 
        CancellationToken cancellationToken)
    {
        var result = await service.GetIncomeDetails(incomeId, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateIncome(
        IIncomePublicService service,
        [FromBody] CreateIncomeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateIncome(request, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> EditIncome(
        IIncomePublicService service,
        [FromBody] IncomeDto model, 
        int incomeId,
        CancellationToken cancellationToken)
    {
        var result = await service.EditIncome(model, incomeId, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteIncome(
        IIncomePublicService service,
        int incomeId,
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteIncome(incomeId, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> AddProductToIncome(
        IIncomePublicService service,
        int incomeId, 
        [FromBody] IEnumerable<ProductToIncomeRequest> requests,
        CancellationToken cancellationToken)
    {
        var result = await service.AddProductToIncome(incomeId, requests, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteProductFromIncome(
        IIncomePublicService service,
        int incomeId, 
        [FromBody] IEnumerable<ProductToIncomeRequest> requests, 
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteProductFromIncome(incomeId, requests, cancellationToken);

        return Results.Ok(result);
    }
}