using InmobiliaryMgmt.Application.Interfaces;
using InmobiliaryMgmt.Application.DTOs.User;
using InmobiliaryMgmt.Domain.Interfaces;

namespace InmobiliaryMgmt.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    
    public async Task<UserResponseDto?> GetProfileByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId); 
        
        if (user == null)
            return null;

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            DocumentNumber = user.DocumentNumber,
            RoleName = user.Role?.Name ?? "N/A", 
            DocTypeName = user.DocType?.Name ?? "N/A"
        };
    }

    public async Task<UserResponseDto> UpdateProfileAsync(int userId, UserUpdate dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado."); 
        }

        user.Name = dto.Name ?? user.Name;
        user.LastName = dto.LastName ?? user.LastName;
        user.DocumentNumber = dto.DocumentNumber ?? user.DocumentNumber;

        await _userRepository.UpdateAsync(user);
        
        var updatedProfile = await GetProfileByIdAsync(user.Id);
        
        return updatedProfile ?? 
               throw new InvalidOperationException("Error al recuperar el perfil actualizado después de la actualización.");
    }
}