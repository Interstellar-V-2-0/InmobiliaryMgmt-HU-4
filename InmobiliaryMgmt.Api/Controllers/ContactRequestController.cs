using System.Security.Claims;
using InmobiliaryMgmt.Application.DTOs;
using InmobiliaryMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InmobiliaryMgmt.Api.Extensions; 
namespace InmobiliaryMgmt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactRequestController : ControllerBase
{
    private readonly IContactRequestService _contactRequestService;

    public ContactRequestController(IContactRequestService contactRequestService)
    {
        _contactRequestService = contactRequestService;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(ContactRequestCreateDto dto)
    {
        int userId;
        try
        {
            // Usamos la extensión en lugar de User.FindFirstValue(...)
            userId = this.GetUserId(); 
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }

        try
        {
            var result = await _contactRequestService.CreateAsync(userId, dto);
            return StatusCode(StatusCodes.Status201Created, result); // Usar 201 Created
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
}