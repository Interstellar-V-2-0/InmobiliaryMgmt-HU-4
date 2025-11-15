namespace InmobiliaryMgmt.Application.DTOs.User;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string DocTypeName { get; set; } = string.Empty;
}