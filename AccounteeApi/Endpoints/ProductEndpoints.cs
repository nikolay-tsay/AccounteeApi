using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("Product", GetProducts)
            .RequireAuthorization()
            .Produces<PagedList<ProductDto>>();

        app.MapGet("Product/{productId}", GetProductById)
            .RequireAuthorization()
            .Produces<ProductDto>();

        app.MapPost("Product", CreateProduct)
            .RequireAuthorization()
            .Produces<ProductDto>();

        app.MapPut("Product/{productId}", EditProduct)
            .RequireAuthorization()
            .Produces<ProductDto>();       
        
        app.MapPut("Product/{productId}/Category/{categoryId}", ChangeProductCategory)
            .RequireAuthorization()
            .Produces<ProductDto>();
        
        app.MapDelete("Product/{productId}", DeleteProduct)
            .RequireAuthorization()
            .Produces<bool>();
    }
    
    private static async Task<IResult> GetProducts(
        IProductPublicService service,
        [AsParameters] OrderFilter orderFilter, 
        [AsParameters] PageFilter pageFilter, 
        CancellationToken cancellationToken)
    {
        var result = await service.GetProducts(orderFilter, pageFilter, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetProductById(
        IProductPublicService service,
        int productId,
        CancellationToken cancellationToken)
    {
        var result = await service.GetProductById(productId, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> CreateProduct(
        IProductPublicService service,
        [FromBody] ProductDto model,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateProduct(model, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> EditProduct(
        IProductPublicService service,
        int productId, 
        [FromBody] ProductDto model,
        CancellationToken cancellationToken)
    {
        var result = await service.EditProduct(productId, model, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> ChangeProductCategory(
        IProductPublicService service,
        int productId, 
        int categoryId,
        CancellationToken cancellationToken)
    {
        var result = await service.ChangeProductCategory(productId, categoryId, cancellationToken);
        
        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteProduct(
        IProductPublicService service,
        int productId,
        CancellationToken cancellationToken)
    {
        var result = await service.DeleteProduct(productId, cancellationToken);
        
        return Results.Ok(result);
    }
}