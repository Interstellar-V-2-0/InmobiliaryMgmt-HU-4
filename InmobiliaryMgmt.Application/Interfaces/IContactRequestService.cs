using InmobiliaryMgmt.Application.DTOs;

namespace InmobiliaryMgmt.Application.Interfaces;

public interface IContactRequestService
{
    Task<ContactRequestResponseDto> CreateAsync(int userId, ContactRequestCreateDto dto);
}