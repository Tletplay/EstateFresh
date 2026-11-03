using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EstateFresh.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:SenderEmail"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(
            _configuration["EmailSettings:SmtpServer"],
            int.Parse(_configuration["EmailSettings:SmtpPort"]),
            SecureSocketOptions.StartTls
        );
        await smtp.AuthenticateAsync(
            _configuration["EmailSettings:SenderEmail"],
            _configuration["EmailSettings:SenderPassword"]
        );
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}