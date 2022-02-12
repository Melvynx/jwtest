using System.ComponentModel.DataAnnotations;

namespace Jwtest;

public class AuthenticateRequest
{
    [Required] public string Username { get; set; }

    [Required] public string Password { get; set; }
}