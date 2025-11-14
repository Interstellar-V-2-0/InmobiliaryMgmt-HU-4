namespace InmobiliaryMgmt.Application.DTOs.User;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime RegisterDate { get; set; }
}