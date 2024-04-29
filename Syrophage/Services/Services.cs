using System.Net.Mail;
using System.Net;

namespace Syrophage.Services
{
    public class Services : IServices
    {


        public bool SendThanksEmail(string email)
        {
            var fromEmail = new MailAddress("syrophage@gmail.com", "SYROPHAGE");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "unrjpaiutcscfree"; // This should be replaced with your actual email password
            string subject = "Thank You for Your Interaction!";

            string body = $"<br/>Dear Customer,<br/><br/>We sincerely appreciate your interaction with us. Thank you for your support!<br/>" +
                          $"You Sucribed to Our New Letter!<br/><br/>Best regards,<br/>SYROPHAGE";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587, // Gmail SMTP port
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                try
                {
                    smtp.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        /*----------------------------------------------------------------------------------------------------------*/

        public bool SendRegistrationEmail(string email, string RegId)
        {
            var fromEmail = new MailAddress("syrophage@gmail.com", "SYROPHAGE");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "unrjpaiutcscfree"; // This should be replaced with your actual email password
            string subject = "Thank You for Your Regristration!";

            string body = $"<br/>Dear Customer,<br/><br/>Thank you for registering with us! We are thrilled to have you join our community.<br/>" +
         $"Register ID: {RegId}<br/>" +
         $"Your registration was successful. However, please note that your account is pending approval by our admin.<br/>" +
         $"Once your account has been approved by the admin, you will receive a confirmation email.<br/><br/>" +
         $"As a registered member, you'll gain access to exclusive benefits, special offers, and personalized services tailored just for you.<br/>" +
         $"Feel free to explore our website and discover all that we have to offer.<br/><br/>" +
         $"Should you have any questions or need assistance, don't hesitate to reach out to us. We're always happy to help!<br/><br/>" +
         $"Best regards,<br/>SYROPHAGE";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587, // Gmail SMTP port
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                try
                {
                    smtp.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        /*----------------------------------------------------------------------------------------------------------*/
        public bool SendActivationEmail(string email, string password)
        {
            var fromEmail = new MailAddress("syrophage@gmail.com", "SYROPHAGE");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "unrjpaiutcscfree"; // This should be replaced with your actual email password
            string subject = "Welcome to Our Community!";

            // Email body with login credentials
            string body = $"<br/>Dear Customer,<br/><br/>Thank you for registering with us! We are thrilled to have you join our community.<br/>" +
                $"Your registration was successful. Here are your login credentials:<br/>" +
                $"Email: {email}<br/>" +
                $"Password: {password}<br/>" +
                $"Please use these credentials to log in to your account.<br/><br/>" +
                $"As a registered member, you'll gain access to exclusive benefits, special offers, and personalized services tailored just for you.<br/>" +
                $"Feel free to explore our website and discover all that we have to offer.<br/><br/>" +
                $"Should you have any questions or need assistance, don't hesitate to reach out to us. We're always happy to help!<br/><br/>" +
                $"Best regards,<br/>SYROPHAGE";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587, // Gmail SMTP port
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                try
                {
                    smtp.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        
    }
}
