using System.ComponentModel.DataAnnotations;

namespace Jwtest;

public class LoginRequest
{
    [Required] public string Email { get; set; }

    [Required] public string Password { get; set; }
}