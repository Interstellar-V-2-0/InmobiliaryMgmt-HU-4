namespace InmobiliaryMgmt.Infrastructure.Data.Config;

public class EmailConfiguration
{
    public string SmtpUser { get; set; } = string.Empty;
    
    public string SmtpAppPassword { get; set; } = string.Empty;

    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public bool EnableSsl { get; set; } = true;
    public string SenderName { get; set; } = string.Empty;
}