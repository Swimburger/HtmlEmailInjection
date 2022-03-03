using System.Net;
using System.Net.Mail;

namespace HtmlEmailInjection;

public class EmailSender
{
    private readonly string fromEmailAddress;
    private readonly string smtpHost;
    private readonly int smtpPort;
    private readonly string smtpUserName;
    private readonly string smtpPassword;

    public EmailSender(IConfiguration configuration)
    {
        fromEmailAddress = configuration["FromEmailAddress"];
        smtpHost = configuration["SmtpHost"];
        smtpPort = configuration.GetValue<int>("SmtpPort");
        smtpUserName = configuration["SmtpUserName"];
        smtpPassword = configuration["SmtpPassword"];
    }

    public async Task SendEmailAsync(string subject, string htmlBody, string toEmail)
    {
        using var message = new MailMessage();
        message.From = new MailAddress(fromEmailAddress);
        message.To.Add(new MailAddress(toEmail));
        message.Subject = subject;

        message.Body = htmlBody;
        message.IsBodyHtml = true;

        using var client = new SmtpClient(host: smtpHost,port: smtpPort);
        client.Credentials = new NetworkCredential(userName: smtpUserName,password: smtpPassword);

        await client.SendMailAsync(message);
    }
}