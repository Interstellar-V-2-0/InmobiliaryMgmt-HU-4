using System.ComponentModel.DataAnnotations;

namespace InmobiliaryMgmt.Application.DTOs.User;

public class RefreshRequest
{
    [Required] public string RefreshToken { get; set; } = string.Empty;
}