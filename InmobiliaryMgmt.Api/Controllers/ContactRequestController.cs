using System.Security.Claims;
using InmobiliaryMgmt.Application.DTOs;
using InmobiliaryMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliaryMgmt.Api.Controllers;

[ApiController]
[Route("api/ContactRequest")]
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
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _contactRequestService.CreateAsync(userId, dto);

        return Ok(result);
    }
}