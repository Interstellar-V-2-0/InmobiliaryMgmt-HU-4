using InmobiliaryMgmt.Application.DTOs.User;

namespace InmobiliaryMgmt.Application.Interfaces
{
    public interface IUserService
    {
        Task<string> Register(string name, string lastName, string email, string password, int roleId, int docTypeId);
        Task<(string? AccessToken, string? RefreshToken)> Login(string email, string password);
        Task<(string? AccessToken, string? RefreshToken)> RefreshToken(string refreshToken);
        
        Task<UserResponseDto?> GetProfileByIdAsync(int userId);
        Task<UserResponseDto> UpdateProfileAsync(int userId, UserUpdate dto);
    }
}