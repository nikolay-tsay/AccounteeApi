using AccounteeCQRS.Requests.Product;
using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGroup("product")
            .MapEndpoints()
            .RequireAuthorization();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetProducts)
            .Produces<PagedList<ProductResponse>>();

        group.MapGet("/{productId}", GetProductById)
            .Produces<ProductResponse>();

        group.MapPost("/", CreateProduct)
            .Produces<ProductResponse>();

        group.MapPut("/", EditProduct)
            .Produces<ProductResponse>();       
        
        group.MapPut("/{productId}/category/{categoryId}", ChangeProductCategory)
            .Produces<ProductResponse>();
        
        group.MapDelete("/{productId}", DeleteProduct)
            .Produces<bool>();

        return group;
    }
    
    private static async Task<IResult> GetProducts(
        IMediator mediator,
        [FromQuery] string? searchValue,
        [AsParameters] OrderFilter orderFilter, 
        [AsParameters] PageFilter pageFilter, 
        CancellationToken cancellationToken)
    {

        var query = new GetProductsQuery(searchValue, orderFilter, pageFilter);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetProductById(
        IMediator mediator,
        int productId,
        CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(productId);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> CreateProduct(
        IMediator mediator,
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> EditProduct(
        IMediator mediator,
        [FromBody] EditProductCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Ok(result);
    }
    
    private static async Task<IResult> ChangeProductCategory(
        IMediator mediator,
        int productId, 
        int categoryId,
        CancellationToken cancellationToken)
    {
        var command = new ChangeProductCategoryCommand(productId, categoryId);
        var result = await mediator.Send(command, cancellationToken);
        
        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteProduct(
        IMediator mediator,
        int productId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(productId);
        var result = await mediator.Send(command, cancellationToken);
        
        return Results.Ok(result);
    }
}