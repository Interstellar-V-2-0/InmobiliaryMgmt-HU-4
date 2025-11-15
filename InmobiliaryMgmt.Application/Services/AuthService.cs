using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Repositories;
using InmobiliaryMgmt.Application.Interfaces;
using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InmobiliaryMgmt.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _configuration = configuration;
    }
    
    public async Task<string> Register(string name, string lastName, string email, string password, int roleId, int docTypeId)
    {
        var exists = await _userRepository.EmailExistsAsync(email);
        if (exists)
            return "El correo ya se encuentra registrado";

        var user = new User
        {
            Name = name,
            LastName = lastName,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            RoleId = roleId,
            DocTypeId = docTypeId,
            DocumentNumber = "0000000000", 
            RegisterDate = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        return "Usuario registrado correctamente";
    }
    
    public async Task<(string? AccessToken, string? RefreshToken)> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return (null, null);

        var accessToken = GenerateJwtToken(user);
        var refreshToken = await GenerateRefreshToken(user);

        return (accessToken, refreshToken);
    }
    

    public async Task<(string? AccessToken, string? RefreshToken)> RefreshToken(string token)
    {

        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenWithUserAsync(token);

        if (refreshTokenEntity == null || refreshTokenEntity.ExpiryDate < DateTime.UtcNow)
            return (null, null);

        refreshTokenEntity.IsRevoked = true;
        await _refreshTokenRepository.UpdateAsync(refreshTokenEntity);

        var accessToken = GenerateJwtToken(refreshTokenEntity.User!);
        var newRefreshToken = await GenerateRefreshToken(refreshTokenEntity.User!);

        return (accessToken, newRefreshToken);
    }
    
    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("Name", user.Name)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"], 
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(3),
            claims: claims,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private async Task<string> GenerateRefreshToken(User user)
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        await _refreshTokenRepository.CreateAsync(refreshToken);

        return refreshToken.Token;
    }
}