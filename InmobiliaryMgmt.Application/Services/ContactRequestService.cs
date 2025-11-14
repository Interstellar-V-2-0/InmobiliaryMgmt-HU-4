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

        await _contactRequestRepository.AddAsync(request);
        await _contactRequestRepository.SaveChangesAsync();

        // Enviar correo al propietario
        await _emailService.SendEmailAsync(
            property.User.Email,
            "Nueva solicitud de contacto",
            $"Mensaje del usuario:\n\n{dto.Message}"
        );

        // Respuesta de éxito
        return new ContactRequestResponseDto
        {
            Id = request.Id,
            PropertyId = request.PropertyId,
            UserId = request.UserId,
            Message = request.Message,
            SentDate = request.SentDate
        };
    }
}