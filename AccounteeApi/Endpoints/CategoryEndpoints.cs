using AccounteeApi.Filters;
using AccounteeDomain.Entities.Enums;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("Category", GetCategories)
            .RequireAuthorization()
            .Produces<PagedList<CategoryDto>>();

        app.MapPost("Category", CreateCategory)
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<CategoryDto>>()
            .Produces<CategoryDto>();
        
        app.MapPut("Category/{id}", EditCategory)
            .RequireAuthorization()
            .Produces<CategoryDto>();

        app.MapDelete("Category/{id}", DeleteCategory)
            .RequireAuthorization()
            .Produces<bool>();
    }
    
    private static async Task<IResult> GetCategories(
        ICategoryPublicService service,
        [FromQuery] string? searchValue,
        [AsParameters] OrderFilter orderFilter,
        [AsParameters] PageFilter pageFilter, 
        [AsParameters] CategoryTargets target,
        CancellationToken cancellationToken)
    {
        var result = await service.GetCategories(searchValue, orderFilter, pageFilter, target, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateCategory(
        ICategoryPublicService service,
        [FromBody] CategoryDto model,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateCategory(model, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> EditCategory(
        ICategoryPublicService service, 
        int id, 
        [AsParameters] CategoryTargets target,
        [FromBody] CategoryDto model,
        CancellationToken cancellationToken)
    {
        var result = await service.EditCategory(id, model, target, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteCategory(
        ICategoryPublicService service,
        int id, 
        [AsParameters] CategoryTargets target, 
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteCategory(target, id, cancellationToken);

        return Results.Ok(result);
    }
}