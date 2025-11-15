using InmobiliaryMgmt.Application.DTOs.Property;
using InmobiliaryMgmt.Application.Interfaces;
using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;
using InmobiliaryMgmt.Application.DTOs; // para reutilizar DTOs

namespace InmobiliaryMgmt.Application.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUserRepository _userRepository;

    public PropertyService(IPropertyRepository propertyRepository, IUserRepository userRepository)
    {
        _propertyRepository = propertyRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<PropertyResponseDto>> GetAllWithImagesAsync()
    {
        var properties = await _propertyRepository.GetAllWithImagesAsync();
        
        return properties.Select(p => MapPropertyToResponseDto(p)).ToList();
    }

    public async Task<PropertyResponseDto?> GetByIdAsync(int id)
    {
        var property = await _propertyRepository.GetByIdAsync(id); 
        
        if (property == null) return null;
        
        return MapPropertyToResponseDto(property);
    }

    public async Task<PropertyResponseDto> CreateAsync(int userId, PropertyUpsertDto dto)
    {
        if (await _propertyRepository.ExistsByTitleAsync(dto.Title))
            throw new Exception("Ya existe una propiedad con ese título.");

        var property = new Property
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            Address = dto.Address,
            UserId = userId
        };

        var createdProperty = await _propertyRepository.CreateAsync(property);
        
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("Propietario no encontrado después de la creación.");

        createdProperty.User = user; 

        return MapPropertyToResponseDto(createdProperty);
    }

    public async Task<PropertyResponseDto> UpdateAsync(int propertyId, int userId, PropertyUpsertDto dto)
    {
        var property = await _propertyRepository.GetByIdAsync(propertyId);
        
        if (property == null)
            throw new KeyNotFoundException($"Propiedad con ID {propertyId} no encontrada.");
        
        if (property.UserId != userId)
            throw new UnauthorizedAccessException("Solo el propietario puede actualizar esta propiedad.");
        

        property.Title = dto.Title;
        property.Description = dto.Description;
        property.Price = dto.Price;
        property.Address = dto.Address;

        var updatedProperty = await _propertyRepository.UpdateAsync(property);
        

        var propertyWithDetails = await _propertyRepository.GetByIdAsync(updatedProperty.Id);
        
        return MapPropertyToResponseDto(propertyWithDetails!);
    }

    public async Task<bool> DeleteAsync(int propertyId, int userId)
    {
        var property = await _propertyRepository.GetByIdAsync(propertyId);
        
        if (property == null) return false;
        
        if (property.UserId != userId)
            throw new UnauthorizedAccessException("Solo el propietario puede eliminar esta propiedad.");
        
        await _propertyRepository.DeleteAsync(property);
        return true;
    }
    
    public async Task<IEnumerable<PropertyResponseDto>> GetByRangePriceAsync(decimal min, decimal max)
    {
        var properties = await _propertyRepository.GetByRangePriceAsync(min, max);
        return properties.Select(p => MapPropertyToResponseDto(p)).ToList();
    }
    
    private static PropertyResponseDto MapPropertyToResponseDto(Property property)
    {
        return new PropertyResponseDto
        {
            Id = property.Id,
            Title = property.Title,
            Description = property.Description,
            Price = property.Price,
            Address = property.Address,
            UserId = property.UserId,

            OwnerName = $"{property.User?.Name} {property.User?.LastName}", 
            OwnerEmail = property.User?.Email ?? "N/A", 
            PropertyImages = property.PropertyImages.Select(img => new PropertyImageResponseDto
            {
                Id = img.Id,
                Url = img.Url
            }).ToList()
        };
    }
}