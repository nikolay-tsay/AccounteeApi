using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccounteeApi.Controllers.Base;

[Authorize]
[ApiController]
public class BaseController : ControllerBase { }