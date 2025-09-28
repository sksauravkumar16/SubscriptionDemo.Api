using System.ComponentModel.DataAnnotations;

namespace SubscriptionDemo.Api.DTOs;

public class RegisterDto
{
    [Required] public string Username { get; set; } = null!;
    [Required] [MinLength(6)] public string Password { get; set; } = null!;
}

public class LoginDto
{
    [Required] public string Username { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}
