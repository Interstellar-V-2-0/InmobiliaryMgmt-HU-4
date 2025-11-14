using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InmobiliaryMgmt.Application.Interfaces;
using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace InmobiliaryMgmt.Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // Registro
    public async Task<string> Register(string name, string lastName, string email, string password, int roleId, int docTypeId)
    {
        var exists = await _context.Users.AnyAsync(x => x.Email == email);
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
            RegisterDate = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return "Usuario registrado correctamente";
    }

    // Login
    public async Task<string?> Login(string email, string password)
    {
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
            return null;

        bool validPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!validPassword)
            return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? ""),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("Name", user.Name)
        };

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddHours(3),
            claims: claims,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
    