using InmobiliaryMgmt.Application.DTOs.User;

namespace InmobiliaryMgmt.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto?> GetProfileByIdAsync(int userId);
        Task<UserResponseDto> UpdateProfileAsync(int userId, UserUpdate dto);
    }
}