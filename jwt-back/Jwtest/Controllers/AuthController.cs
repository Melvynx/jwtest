using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Jwtest.Helpers;
using Jwtest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Jwtest.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppSettings _jwtConfig;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtContext _context;

    public AuthController(UserManager<IdentityUser> userManager, IOptionsMonitor<AppSettings> optionsMonitor, JwtContext context)
    {
        _userManager = userManager;
        _jwtConfig = optionsMonitor.CurrentValue;
        _context = context;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest user)
    {
        var newUser = new IdentityUser {Email = user.Email, UserName = user.Name};
        var isCreated = await _userManager.CreateAsync(newUser, user.Password);

        if (!isCreated.Succeeded)
            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Message = string.Join(Environment.NewLine, isCreated.Errors.Select(e => e.Description).ToList())
            });

        var jwtToken = GenerateJwtToken(newUser);

        return Ok(new RegistrationResponse
        {
            Result = true,
            Token = jwtToken
        });
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest user)
    {
        var existingUser = await _userManager.FindByEmailAsync(user.Email);

        if (existingUser == null)
            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Message = "Invalid authentication request"
            });

        var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

        if (!isCorrect)
            return BadRequest(new RegistrationResponse
            {
                Result = false,
                Message = "Invalid authentication request"
            });

        var jwtToken = GenerateJwtToken(existingUser);

        return Ok(new RegistrationResponse
        {
            Result = true,
            Token = jwtToken
        });
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(6),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        return jwtTokenHandler.WriteToken(token);
    }
    
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMe([FromQuery(Name = "isPayload")] string isPayload)
    {
        var id = HttpContext.User.Claims.FirstOrDefault(e => e.Type == "Id")?.Value;
        var sub = HttpContext.User.Claims.ElementAt(1).Value;
        var email = HttpContext.User.Claims.ElementAt(2).Value;;

        if (isPayload != "true")
        {
            var user = _context.Users.FirstOrDefault(e => e.Id == id);
            return Ok(user);
        };

        var obj = new
        {
            Id = id,
            UserName = sub,
            Email = email
        };
        return Ok(obj);

        // var user = _userService.GetById(userId);
    }
}