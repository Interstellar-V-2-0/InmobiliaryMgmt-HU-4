using InmobiliaryMgmt.Application.DTOs.User;

namespace InmobiliaryMgmt.Application.Interfaces;

public interface IAuthService
{
    Task<string> Register(string name, string lastName, string email, string password, int roleId, int docTypeId);
    Task<(string? AccessToken, string? RefreshToken)> Login(string email, string password);
    Task<(string? AccessToken, string? RefreshToken)> RefreshToken(string token);
}