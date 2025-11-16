using System.ComponentModel.DataAnnotations;

namespace InmobiliaryMgmt.Application.DTOs.User;

public class RegisterUserDto
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    [Required] public int RoleId { get; set; }
    [Required] public int DocTypeId { get; set; }
    
}