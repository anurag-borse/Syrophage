namespace Syrophage.Services
{
    public interface ISenderEmail
    {
        Task SendEmailAsync(string ToEmail, string Subject, string Body, Stream attachmentStream, string attachmentName, bool IsBodyHtml = false);
    }
}
