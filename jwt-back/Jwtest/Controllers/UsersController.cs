using Jwtest.Helpers;
using Jwtest.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jwtest.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
        // var response = _userService.Authenticate(model);
        //
        // if (response == null)
        //     return BadRequest(new {message = "Username or password is incorrect"});

        return Ok(null);
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetAll()
    {
        // var users = _userService.GetAll();
        return Ok(null);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMe([FromQuery(Name = "isPayload")] string isPayload)
    {
        var userId = Converter.ObjectToInt(HttpContext.Items["User"]);

        if (isPayload != "true") return Ok(null);

        var obj = new
        {
            Id = userId,
            Username = HttpContext.Items["Username"],
        };
        return Ok(obj);

        // var user = _userService.GetById(userId);
    }
}