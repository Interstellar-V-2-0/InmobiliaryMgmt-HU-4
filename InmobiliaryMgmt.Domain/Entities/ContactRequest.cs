namespace InmobiliaryMgmt.Domain.Entities;

public class ContactRequest
{
    public int Id { get; set; }
    
    public int PropertyId { get; set; }
    public Property? Property { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }

    public required string Message { get; set; }
    public DateTime SentDate { get; set; }
}