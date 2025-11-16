using InmobiliaryMgmt.Application.DTOs.Property;

namespace InmobiliaryMgmt.Application.Interfaces;

public interface IPropertyService
{
    Task<IEnumerable<PropertyResponseDto>> GetAllWithImagesAsync();
    Task<PropertyResponseDto?> GetByIdAsync(int id);
    Task<PropertyResponseDto> CreateAsync(int userId, PropertyUpsertDto dto);
    Task<PropertyResponseDto> UpdateAsync(int propertyId, int userId, PropertyUpsertDto dto);
    Task<bool> DeleteAsync(int propertyId, int userId);
    Task<IEnumerable<PropertyResponseDto>> GetByRangePriceAsync(decimal min, decimal max);
}