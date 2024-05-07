
using System.Net.Mail;
using System.Net;

namespace Syrophage.Services
{
    public class EmailService : ISenderEmail
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string ToEmail, string Subject, string Body, Stream attachmentStream, string attachmentName, bool IsBodyHtml = false)
        {
            string MailServer = _configuration["EmailSettings:MailServer"] ?? string.Empty;
            string FromEmail = _configuration["EmailSettings:FromEmail"] ?? string.Empty;
            string Password = _configuration["EmailSettings:Password"] ?? string.Empty;
            int Port = int.Parse(_configuration["EmailSettings:MailPort"]);

            var client = new SmtpClient(MailServer, Port)
            {
                Credentials = new NetworkCredential(FromEmail, Password),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Body)
            {
                IsBodyHtml = IsBodyHtml
            };

            if (attachmentStream != null)
            {
                mailMessage.Attachments.Add(new Attachment(attachmentStream, attachmentName));
            }

            return client.SendMailAsync(mailMessage);
        }
    }
}
