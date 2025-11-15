using InmobiliaryMgmt.Api.Extensions; // Importamos la extensión
using InmobiliaryMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InmobiliaryMgmt.Application.DTOs.User;

namespace InmobiliaryMgmt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    

    
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserProfile()
    {
        int userId;
        try
        {

            userId = this.GetUserId(); 
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        var userProfile = await _userService.GetProfileByIdAsync(userId); 
        
        if (userProfile == null)
            return NotFound("Perfil de usuario no encontrado.");

        return Ok(userProfile);
    }
    
    [HttpPut("me")]
    public async Task<IActionResult> UpdateCurrentUserProfile([FromBody] UserUpdate dto)
    {
        int userId;
        try
        {

            userId = this.GetUserId(); 
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        try
        {
            var updatedUser = await _userService.UpdateProfileAsync(userId, dto);
            return Ok(updatedUser);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Usuario no encontrado.");
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}