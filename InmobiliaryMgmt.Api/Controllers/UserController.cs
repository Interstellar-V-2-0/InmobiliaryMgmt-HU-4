using InmobiliaryMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

    private int? GetUserIdFromClaims()
    {
        var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(claimValue) || !int.TryParse(claimValue, out var userId))
            return null;
        return userId;
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserProfile()
    {
        var userId = GetUserIdFromClaims();
        if (userId == null) 
            return Unauthorized("Token inválido o ID de usuario faltante.");
        
        var userProfile = await _userService.GetProfileByIdAsync(userId.Value); 
        
        if (userProfile == null)
            return NotFound("Perfil de usuario no encontrado.");

        return Ok(userProfile);
    }
    
    [HttpPut("me")]
    public async Task<IActionResult> UpdateCurrentUserProfile([FromBody] UserUpdate dto)
    {
        var userId = GetUserIdFromClaims();
        if (userId == null) 
            return Unauthorized("Token inválido o ID de usuario faltante.");
        
        try
        {
            var updatedUser = await _userService.UpdateProfileAsync(userId.Value, dto);
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