using AccounteeApi.Controllers.Base;
using AccounteeDomain.Entities.Enums;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Controllers;

public class CategoryController : BaseController
{
    private ICategoryPublicService CategoryPublicService { get; }

    public CategoryController(ICategoryPublicService categoryPublicService)
    {
        CategoryPublicService = categoryPublicService;
    }

    [HttpGet("Category")]
    [ProducesResponseType(typeof(PagedList<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories([FromQuery] PageFilter filter, [FromQuery] CategoryTargets target, CancellationToken cancellationToken)
    {
        var result = await CategoryPublicService.GetCategories(filter, target, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("Category")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateCategory([FromBody]CategoryDto model, CancellationToken cancellationToken)
    {
        var result = await CategoryPublicService.CreateCategory(model, cancellationToken);

        return Ok(result);
    }
    
    [HttpPut("Category/{id}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditCategory(int id, [FromQuery] CategoryTargets target, [FromBody] CategoryDto model, CancellationToken cancellationToken)
    {
        var result = await CategoryPublicService.EditCategory(id, model, target, cancellationToken);

        return Ok(result);
    }
    
    [HttpDelete("Category/{id}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCategory(int id, [FromQuery] CategoryTargets target, CancellationToken cancellationToken)
    {
        var result = await CategoryPublicService.DeleteCategory(target, id, cancellationToken);

        return Ok(result);
    }
}