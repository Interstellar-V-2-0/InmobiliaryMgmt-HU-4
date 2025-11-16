using InmobiliaryMgmt.Application.DTOs;
using InmobiliaryMgmt.Application.Interfaces;
using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;

namespace InmobiliaryMgmt.Application.Services;

public class ContactRequestService : IContactRequestService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IContactRequestRepository _contactRequestRepository;
    private readonly IEmailService _emailService;

    public ContactRequestService(
        IPropertyRepository propertyRepository,
        IContactRequestRepository contactRequestRepository,
        IEmailService emailService
        )
    {
        _propertyRepository = propertyRepository;
        _contactRequestRepository = contactRequestRepository;
        _emailService = emailService;
    }

    public async Task<ContactRequestResponseDto> CreateAsync(int userId, ContactRequestCreateDto dto)
    {
        var property = await _propertyRepository.GetByIdAsync(dto.PropertyId); 

        if (property == null)
            throw new Exception("La propiedad no existe.");

        if (property.User == null)
            throw new Exception("La propiedad no tiene propietario asignado.");

        var request = new ContactRequest
        {
            PropertyId = dto.PropertyId,
            UserId = userId,
            Message = dto.Message,
            SentDate = DateTime.UtcNow,
        };
        
        var createdRequest = await _contactRequestRepository.CreateAsync(request); 
        
        var recipientEmail = property.User.Email;

        if (!string.IsNullOrEmpty(recipientEmail))
        {
            await _emailService.SendEmailAsync(
                recipientEmail,
                "Nueva solicitud de contacto",
                $"Mensaje del usuario:\n\n{dto.Message}"
            );
        }
        else
        {
            Console.WriteLine($"[ADVERTENCIA] No se pudo enviar notificación: El propietario de la propiedad {property.Id} (Usuario ID: {property.User.Id}) no tiene un correo electrónico válido registrado.");
        }
        
        return new ContactRequestResponseDto
        {
            Id = createdRequest.Id,
            PropertyId = createdRequest.PropertyId,
            UserId = createdRequest.UserId,
            Message = createdRequest.Message,
            SentDate = createdRequest.SentDate
        };
    }
}