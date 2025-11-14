using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InmobiliaryMgmt.Application.Services;

public class UserServices
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UserServices(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    //Registro
    public async Task<String> Register(String Name, String Email, String Password, int RoleId)
    {
        var exists = await _context.Users.AnyAsync(x => x.Email == Email);
        if (exists)
            return "El correo ya se encuentra registrado";

        var user = new User
        {
            Name = Name,
            Email = Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password),
            RoleId = RoleId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return "usuario resgistrado correctamente";
    }

    //Login
    public async Task<String> Register(String Email, String Password)
    {
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == Email);

        if (user == null)
            return null;
        
        bool validPassword = BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash);
        if (!validPassword)
            return null;

        return GenerateJwtToken(user);
    }

    // -------------------------- JWT --------------------------
    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name),
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
