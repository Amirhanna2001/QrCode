using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MimeKit.Text;
using QrCode.API.DTOs;

namespace QrCode.API.Services;

public class EmailServices : IEmailServices
{
    private readonly IConfiguration config;

    public EmailServices(IConfiguration config)
    {
        this.config = config;
    }

    public void SendEmail(EmailDTO dto)
    {
        MimeMessage email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(config["EmailSender:Email"]));
        email.To.Add(MailboxAddress.Parse(dto.To));
        email.Subject = "Email For Verify your Registration";
        email.Body = new TextPart(TextFormat.Html) { Text = dto.Body };

        using var smtp = new SmtpClient();
        smtp.Connect(config["EmailSender:EmailHost"], 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(config["EmailSender:Email"], config["EmailSender:Password"]);

        smtp.Send(email);
        smtp.Disconnect(true);
    }
}
