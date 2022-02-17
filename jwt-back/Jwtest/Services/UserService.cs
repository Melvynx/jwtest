using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Jwtest.Services;

public interface IUserService
{
    // AuthenticateResponse? Authenticate(AuthenticateRequest model);
    // IEnumerable<User> GetAll();
    // User? GetById(int id);
}

public class UserService : IUserService
{
    private readonly AppSettings _appSettings;
    private readonly JwtContext _context;

    public UserService(JwtContext context, IOptions<AppSettings> appSettings)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }

    // public AuthenticateResponse? Authenticate(AuthenticateRequest model)
    // {
    //     var user = _context.Users.SingleOrDefault(x => x.UserName == model.Username && x.PasswordHash == model.Password);
    //     // return null if user not found
    //     if (user == null) return null;
    //
    //     // authentication successful so generate jwt token
    //     // var token = GenerateJwtToken(user);
    //
    //     return new AuthenticateResponse(user, token);
    // }

    // public IEnumerable<User> GetAll()
    // {
    //     return _context.Users.ToList();
    // }

    // public User? GetById(int id)
    // {
    //     return _context.Users.FirstOrDefault(x => x.Id == id);
    // }

    private string GenerateJwtToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.Username)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}