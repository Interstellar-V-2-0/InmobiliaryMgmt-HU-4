using InmobiliaryMgmt.Application.DTOs.User;
using InmobiliaryMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliaryMgmt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Resuelve a api/auth
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // ====================================
        // REGISTRO DE USUARIO
        // ====================================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            var result = await _userService.Register(
                request.Name,
                request.LastName,
                request.Email,
                request.Password,
                request.RoleId,
                request.DocTypeId
            );

            if (result.Contains("registrado correctamente"))
                return Ok(new { Message = result });
            
            return BadRequest(new { Message = result });
        }

        // ====================================
        // LOGIN
        // ====================================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var (accessToken, refreshToken) = await _userService.Login(request.Email, request.Password);

            if (accessToken == null)
                return Unauthorized(new { Message = "Correo o contraseña incorrecta" });

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        // ====================================
        // REFRESH TOKEN
        // ====================================
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var (newAccessToken, newRefreshToken) = await _userService.RefreshToken(request.RefreshToken);

            if (newAccessToken == null)
                return Unauthorized(new { Message = "Refresh token inválido o expirado" });

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}