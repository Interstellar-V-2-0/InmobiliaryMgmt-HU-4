using InmobiliaryMgmt.Application.DTOs.Property;
using InmobiliaryMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InmobiliaryMgmt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    private int? GetUserIdFromClaims()
    {
        var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(claimValue) || !int.TryParse(claimValue, out var userId))
            return null;
        return userId;
    }
    
    [HttpGet]
    [AllowAnonymous] 
    public async Task<IActionResult> GetAll()
    {
        var properties = await _propertyService.GetAllWithImagesAsync();
        return Ok(properties);
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property == null) return NotFound();
        return Ok(property);
    }
    
    [HttpPost]
    [Authorize(Roles = "Agent,Admin")] 
    public async Task<IActionResult> Create([FromBody] PropertyUpsertDto dto)
    {
        var userId = GetUserIdFromClaims();
        if (userId == null) return Unauthorized("Token inválido o ID de usuario faltante.");

        try
        {
            var createdProperty = await _propertyService.CreateAsync(userId.Value, dto);
            return CreatedAtAction(nameof(GetById), new { id = createdProperty.Id }, createdProperty);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Agent,Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] PropertyUpsertDto dto)
    {
        var userId = GetUserIdFromClaims();
        if (userId == null) return Unauthorized("Token inválido o ID de usuario faltante.");
        
        try
        {
            var updatedProperty = await _propertyService.UpdateAsync(id, userId.Value, dto);
            return Ok(updatedProperty);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Propiedad con ID {id} no encontrada.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message); // HTTP 403 Forbidden
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Agent,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserIdFromClaims();
        if (userId == null) return Unauthorized("Token inválido o ID de usuario faltante.");

        try
        {
            var deleted = await _propertyService.DeleteAsync(id, userId.Value);
            if (!deleted) return NotFound();
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }
}