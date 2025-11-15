using System.ComponentModel.DataAnnotations;

namespace InmobiliaryMgmt.Application.DTOs.User;

public class LoginDto
{
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}