namespace InmobiliaryMgmt.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    
    public int DocTypeId { get; set; }
    public DocType? DocType { get; set; }
    
    public string DocumentNumber { get; set; }
    
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime RegisterDate { get; set; }


    public List<Property> Properties { get; set; } = new List<Property>();
    public List<ContactRequest> ContactRequests { get; set; } = new List<ContactRequest>();
}