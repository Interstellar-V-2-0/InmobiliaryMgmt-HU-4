namespace InmobiliaryMgmt.Application.DTOs;

public class ContactRequestResponseDto
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public DateTime SentDate { get; set; }
}