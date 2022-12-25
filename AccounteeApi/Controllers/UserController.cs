using AccounteeApi.Controllers.Base;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.PublicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Controllers;

public class UserController : BaseController
{
    private IUserPublicService UserPublicService { get; }

    public UserController(IUserPublicService userPublicService)
    {
        UserPublicService = userPublicService;
    }

    [HttpGet("User")]
    [ProducesResponseType(typeof(PagedList<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers([FromQuery]PageFilter filter, CancellationToken cancellationToken)
    {
        var result = await UserPublicService.GetUsers(filter, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("User/{userId}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserById(int userId, CancellationToken cancellationToken)
    {
        var result = await UserPublicService.GetUserById(userId, cancellationToken);

        return Ok(result);
    }

    [HttpPost("User")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterUser([FromBody]RegistrationRequest request, CancellationToken cancellationToken)
    {
        var result = await UserPublicService.RegisterUser(request, cancellationToken);

        return Ok(result);
    }

    [HttpPut("User/{userId}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditUser(int userId, [FromBody] UserDto model, CancellationToken cancellationToken)
    {
        var result = await UserPublicService.EditUser(userId, model, cancellationToken);

        return Ok(result);
    }
    
    [HttpDelete("User/{userId}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        var result = await UserPublicService.DeleteUser(userId, cancellationToken);

        return Ok(result);
    }
    
    [HttpPut("User/{userId}/Role/{roleId}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeUserRole(int roleId, int userId, CancellationToken cancellationToken)
    {
        var result = await UserPublicService.ChangeUserRole(roleId, userId, cancellationToken);

        return Ok(result);
    }
}