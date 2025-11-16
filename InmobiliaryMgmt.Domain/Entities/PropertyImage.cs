namespace InmobiliaryMgmt.Domain.Entities;

public class PropertyImage
{
    public int Id { get; set; }
    public string Url { get; set; }
    
    public string PublicId { get; set; } = string.Empty;
    
    public int PropertyId { get; set; }
    public Property? Property { get; set; }
}