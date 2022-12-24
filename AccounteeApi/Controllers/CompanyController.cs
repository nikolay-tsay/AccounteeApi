using AccounteeApi.Controllers.Base;
using AccounteeDomain.Models;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Controllers;

public class CompanyController : BaseController
{
    private ICompanyPublicService CompanyPublicService { get; }

    public CompanyController(ICompanyPublicService companyPublicService)
    {
        CompanyPublicService = companyPublicService;
    }
    
    [HttpGet("Company")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCompany(CancellationToken cancellationToken)
    {
        var res = await CompanyPublicService.GetCompany(cancellationToken);

        return Ok(res);
    }
    
    [HttpPost("Company")]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateCompany([FromBody]CompanyDto model, CancellationToken cancellationToken)
    {
        var res = await CompanyPublicService.CreateCompany(model, cancellationToken);

        return Ok(res);
    }
    
    [HttpDelete("Company")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCompany(CancellationToken cancellationToken)
    {
        var res = await CompanyPublicService.DeleteCompany(cancellationToken);

        return Ok(res);
    }
    
    [HttpPut("Company")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditCompany([FromBody]CompanyDto model, CancellationToken cancellationToken)
    {
        var res = await CompanyPublicService.EditCompany(model, cancellationToken);

        return Ok(res);
    }
}