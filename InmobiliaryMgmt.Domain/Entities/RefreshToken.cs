using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliaryMgmt.Domain.Entities;


public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
}


