using AccounteeApi.Controllers.Base;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Controllers;

public class ProductController : BaseController
{
    private IProductPublicService ProductPublicService { get; }
    public ProductController(IProductPublicService productPublicService)
    {
        ProductPublicService = productPublicService;
    }

    [HttpGet("Product")]
    [ProducesResponseType(typeof(PagedList<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts([FromQuery] PageFilter filter, CancellationToken cancellationToken)
    {
        var result = await ProductPublicService.GetProducts(filter, cancellationToken);

        return Ok(result);
    }

    [HttpGet("Product/{productId}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductById(int productId, CancellationToken cancellationToken)
    {
        var result = await ProductPublicService.GetProductById(productId, cancellationToken);

        return Ok(result);
    }

    [HttpPost("Product")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDto model, CancellationToken cancellationToken)
    {
        var result = await ProductPublicService.CreateProduct(model, cancellationToken);

        return Ok(result);
    }

    [HttpPut("Product/{productId}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditProduct(int productId, [FromBody] ProductDto model, CancellationToken cancellationToken)
    {
        var result = await ProductPublicService.EditProduct(productId, model, cancellationToken);

        return Ok(result);
        
    }

    [HttpPut("Product/{productId}/Category/{categoryId}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeProductCategory(int productId, int categoryId,
        CancellationToken cancellationToken)
    {
        var result = await ProductPublicService.ChangeProductCategory(productId, categoryId, cancellationToken);
        
        return Ok(result);
    }

    [HttpDelete("Product/{productId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProduct(int productId, CancellationToken cancellationToken)
    {
        var result = await ProductPublicService.DeleteProduct(productId, cancellationToken);
        
        return Ok(result);
    }
}