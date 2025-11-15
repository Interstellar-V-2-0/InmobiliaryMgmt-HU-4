namespace InmobiliaryMgmt.Application.DTOs.Property;

public class PropertyResponseDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string Address { get; set; }
    
    // Información del creador/propietario
    public int UserId { get; set; }
    public required string OwnerName { get; set; } 
    public required string OwnerEmail { get; set; }

    // DTOs anidados para las imágenes
    public List<PropertyImageResponseDto> PropertyImages { get; set; } = new List<PropertyImageResponseDto>();
}