using System.Security.Claims;
using InmobiliaryMgmt.Application.DTOs;
using InmobiliaryMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(claimValue) || !int.TryParse(claimValue, out var userId))
            return Unauthorized("Invalid token or missing user id.");

        try
        {
            var result = await _contactRequestService.CreateAsync(userId, dto);
            return Ok(result); 
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
}