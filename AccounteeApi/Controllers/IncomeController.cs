using AccounteeApi.Controllers.Base;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Contracts.Models;
using AccounteeService.Contracts.Requests;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Controllers;

public class IncomeController : BaseController 
{
    private IIncomePublicService IncomePublicService { get; set; }

    public IncomeController(IIncomePublicService incomePublicService)
    {
        IncomePublicService = incomePublicService;
    }
    
    [HttpGet("Income")]
    [ProducesResponseType(typeof(PagedList<IncomeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncomes([FromQuery] PageFilter filter, CancellationToken cancellationToken)
    {
        var result = await IncomePublicService.GetIncomes(filter, cancellationToken);

        return Ok(result);
    }

    [HttpGet("Income/User/{userId}")]
    [ProducesResponseType(typeof(PagedList<IncomeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserIncomes(int? userId, [FromQuery] PageFilter filter, CancellationToken cancellationToken)
    {
        var result = await IncomePublicService.GetUserIncomes(userId, filter, cancellationToken);

        return Ok(result);
    }

    [HttpGet("Income/{incomeId}")]
    [ProducesResponseType(typeof(IncomeDetailModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncomeDetails(int incomeId, CancellationToken cancellationToken)
    {
        var result = await IncomePublicService.GetIncomeDetails(incomeId, cancellationToken);

        return Ok(result);
    }

    [HttpPost("Income")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await IncomePublicService.CreateIncome(request, cancellationToken);

        return Ok(result);
    }

    [HttpPut("Income/{incomeId}")]
    [ProducesResponseType(typeof(IncomeDetailModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditIncome([FromBody] IncomeDto model, int incomeId, CancellationToken cancellationToken)
    {
        var result = await IncomePublicService.EditIncome(model, incomeId, cancellationToken);

        return Ok(result);
    }
    
    [HttpDelete("Income/{incomeId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteIncome(int incomeId, CancellationToken cancellationToken)
    {
        var result = await IncomePublicService.DeleteIncome(incomeId, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("Income/{incomeId}/Product")]
    [ProducesResponseType(typeof(IncomeDetailModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddProductToIncome(int incomeId, IEnumerable<ProductToIncomeRequest> requests,
        CancellationToken cancellationToken)
    {
        var result = await IncomePublicService.AddProductToIncome(incomeId, requests, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("Income/{incomeId}/Product")]
    [ProducesResponseType(typeof(IncomeDetailModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProductFromIncome(int incomeId, IEnumerable<ProductToIncomeRequest> requests, CancellationToken cancellationToken)
    {
        var result = await IncomePublicService.DeleteProductFromIncome(incomeId, requests, cancellationToken);

        return Ok(result);
    }
}