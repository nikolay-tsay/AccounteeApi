using AccounteeApi.Filters;
using AccounteeDomain.Models;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this WebApplication app)
    {
        app.MapGet("Company", GetCompany)
            .RequireAuthorization()
            .Produces<bool>();

        app.MapPost("Company", CreateCompany)
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<CompanyDto>>()
            .Produces<CompanyDto>();

        app.MapDelete("Company", DeleteCompany)
            .RequireAuthorization()
            .Produces<bool>();
        
        app.MapPut("Company", EditCompany)
            .RequireAuthorization()
            .Produces<bool>();
    }
    
    private static async Task<IResult> GetCompany(
        ICompanyPublicService service,
        CancellationToken cancellationToken)
    {
        var res = await service.GetCompany(cancellationToken);

        return Results.Ok(res);
    }
    
    private static async Task<IResult> CreateCompany(
        ICompanyPublicService service, 
        [FromBody] CompanyDto model, 
        CancellationToken cancellationToken)
    {
        var res = await service.CreateCompany(model, cancellationToken);

        return Results.Ok(res);
    }
    
    private static async Task<IResult> DeleteCompany(
        ICompanyPublicService service,
        CancellationToken cancellationToken)
    {
        var res = await service.DeleteCompany(cancellationToken);

        return Results.Ok(res);
    }
    
    private static async Task<IResult> EditCompany(
        ICompanyPublicService service,
        [FromBody] CompanyDto model,
        CancellationToken cancellationToken)
    {
        var res = await service.EditCompany(model, cancellationToken);

        return Results.Ok(res);
    }
}