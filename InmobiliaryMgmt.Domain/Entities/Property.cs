namespace InmobiliaryMgmt.Domain.Entities;

public class Property
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Address { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }

    public List<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
    public List<ContactRequest> ContactRequests { get; set; } = new List<ContactRequest>();
}