using AccounteeApi.Filters;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class ServiceEndpoints
{
    public static void MapServiceEndpoints(this WebApplication app)
    {
        app.MapGet("Service", GetServices)
            .RequireAuthorization()
            .Produces<PagedList<ServiceDto>>();
        
        app.MapGet("Service/{serviceId}", GetServiceById)
            .RequireAuthorization()
            .Produces<ServiceDto>();

        app.MapPost("Service", CreateService)
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<ServiceDto>>()
            .Produces<ServiceDto>();

        app.MapPut("Service/{serviceId}", EditService)
            .RequireAuthorization()
            .Produces<ServiceDto>();

        app.MapPut("Service/{serviceId}/Category/{categoryId}", ChangeServiceCategory)
            .RequireAuthorization()
            .Produces<ServiceDto>();

        app.MapDelete("Service/{serviceId}", DeleteService)
            .RequireAuthorization()
            .Produces<bool>();
    }

    private static async Task<IResult> GetServices(
        IServicePublicService service,
        [FromQuery] string? searchValue,
        [AsParameters] OrderFilter orderFilter,
        [AsParameters] PageFilter pageFilter,
        CancellationToken cancellationToken)
    {
        var result = await service.GetServices(searchValue, orderFilter, pageFilter, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetServiceById(
        IServicePublicService service,
        int serviceId,
        CancellationToken cancellationToken)
    {
        var result = await service.GetServiceById(serviceId, cancellationToken);
        
        return Results.Ok(result);
    }

    private static async Task<IResult> CreateService(
        IServicePublicService service,
        [FromBody] ServiceDto model,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateService(model, cancellationToken);
        
        return Results.Ok(result);
    }

    private static async Task<IResult> EditService(
        IServicePublicService service,
        int serviceId,
        [FromBody] ServiceDto model,
        CancellationToken cancellationToken)
    {
        var result = await service.EditService(serviceId, model, cancellationToken);
        
        return Results.Ok(result);
    }

    private static async Task<IResult> ChangeServiceCategory(
        IServicePublicService service,
        int serviceId,
        int categoryId,
        CancellationToken cancellationToken)
    {
        var result = await service.ChangeServiceCategory(serviceId, categoryId, cancellationToken);
        
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteService(
        IServicePublicService service,
        int serviceId,
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteService(serviceId, cancellationToken);
        
        return Results.Ok(result);
    }
}