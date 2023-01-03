using AccounteeDomain.Models;
using AccounteeService.Contracts.Requests;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private IAuthPublicService AuthPublicService { get; }
    
    public AuthController(IAuthPublicService authPublicService)
    {
        AuthPublicService = authPublicService;
    }

    [AllowAnonymous]
    [HttpGet("Auth/Login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromQuery] string login, [FromQuery] string password, CancellationToken cancellationToken)
    {
        var result = await AuthPublicService.Login(login, password, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("Auth/Register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request, CancellationToken cancellationToken)
    {
        var result = await AuthPublicService.Register(request, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPut("Auth/ChangePassword")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePassword([FromQuery] int? userId, string oldPwd, string newPwd, CancellationToken cancellationToken)
    {
        var result = await AuthPublicService.ChangePassword(userId, oldPwd, newPwd, cancellationToken);

        return Ok(result);
    }
}