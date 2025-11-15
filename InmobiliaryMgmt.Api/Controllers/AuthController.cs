using InmobiliaryMgmt.Application.DTOs.User;
using InmobiliaryMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliaryMgmt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        {
            _authService = authService; 
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {

            var result = await _authService.Register( 
                request.Name,
                request.LastName,
                request.Email,
                request.Password,
                request.RoleId,
                request.DocTypeId
            );

            if (result.Contains("El correo ya se encuentra registrado")) 
                return BadRequest(new { Message = result });
            
            return Ok(new { Message = result });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {

            var (accessToken, refreshToken) = await _authService.Login(request.Email, request.Password);

            if (accessToken == null)
                return Unauthorized(new { Message = "Correo o contraseña incorrecta" });

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {

            var (newAccessToken, newRefreshToken) = await _authService.RefreshToken(request.RefreshToken);

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