using InmobiliaryMgmt.Domain.Interfaces;
using InmobiliaryMgmt.Infrastructure.Data.Config;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace InmobiliaryMgmt.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailSettings;

    public EmailService(IOptions<EmailConfiguration> emailSettings)
    {
        _emailSettings = emailSettings.Value;
        
        if (string.IsNullOrEmpty(_emailSettings.SmtpUser) || string.IsNullOrEmpty(_emailSettings.SmtpAppPassword))
        {
            throw new ArgumentException("La configuración del remitente SMTP (SmtpUser/SmtpAppPassword) es inválida o nula.");
        }
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_emailSettings.SmtpUser); 
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = body };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            
            await smtp.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpAppPassword);
            
            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al enviar correo electrónico: {ex.Message}");
            throw;
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}