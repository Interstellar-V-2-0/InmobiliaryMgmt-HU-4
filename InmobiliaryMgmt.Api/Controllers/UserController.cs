
using InmobiliaryMgmt.Application.DTOs.User;
using InmobiliaryMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            var result = await _service.Register(
                dto.Name, 
                dto.LastName, 
                dto.Email, 
                dto.Password,
                dto.RoleId,
                dto.DocTypeId
            );

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _service.Login(dto.Email, dto.Password);
            if (token == null) return Unauthorized("Credenciales incorrectas.");
            return Ok(new { token });
        }
    }
}