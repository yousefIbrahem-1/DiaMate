using DiaMate.IServices;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtp = new SmtpClient
        {
            Host = _config["EmailSettings:Host"],
            Port = int.Parse(_config["EmailSettings:Port"]),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _config["EmailSettings:From"],
                _config["EmailSettings:Password"]
            )
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_config["EmailSettings:From"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        await smtp.SendMailAsync(mail);
    }
}
