using InmobiliaryMgmt.Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    public EmailService(IConfiguration config) => _config = config;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpUser = _config["Email:SmtpUser"];
        var smtpPass = _config["Email:SmtpAppPassword"];

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(smtpUser));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Plain) { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(smtpUser, smtpPass);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}