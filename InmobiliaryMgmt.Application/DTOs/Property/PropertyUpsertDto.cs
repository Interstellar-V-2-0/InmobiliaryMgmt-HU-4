namespace InmobiliaryMgmt.Application.DTOs.Property;

public class PropertyUpsertDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string Address { get; set; }
}