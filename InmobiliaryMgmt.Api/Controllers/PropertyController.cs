using InmobiliaryMgmt.Api.Extensions; // Importamos la extensión
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
        int userId;
        try { userId = this.GetUserId(); }
        catch (UnauthorizedAccessException ex) { return Unauthorized(ex.Message); }

        try
        {
            var createdProperty = await _propertyService.CreateAsync(userId, dto);
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
        int userId;
        try { userId = this.GetUserId(); } 
        catch (UnauthorizedAccessException ex) { return Unauthorized(ex.Message); }
        
        try
        {
            var updatedProperty = await _propertyService.UpdateAsync(id, userId, dto);
            return Ok(updatedProperty);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Propiedad con ID {id} no encontrada.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });

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
        int userId;
        try { userId = this.GetUserId(); } 
        catch (UnauthorizedAccessException ex) { return Unauthorized(ex.Message); }

        try
        {
            var deleted = await _propertyService.DeleteAsync(id, userId);
            if (!deleted) return NotFound();
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
        }
    }
}