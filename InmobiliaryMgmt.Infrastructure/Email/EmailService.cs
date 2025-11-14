using InmobiliaryMgmt.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace InmobiliaryMgmt.Infrastructure.Email;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("tu-correo@gmail.com"));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        email.Body = new TextPart(TextFormat.Plain)
        {
            Text = body
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync("tu-correo@gmail.com", "tu-password-app");

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}