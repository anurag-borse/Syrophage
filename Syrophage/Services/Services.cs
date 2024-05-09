using System.Net.Mail;
using System.Net;
using Syrophage.Models.ViewModel;

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

        public string GenerateCouponCode()
        {
            var random = new Random();
            var code = $"SY{random.Next(100, 999)}RO{random.Next(100, 999)}PA{random.Next(100, 999)}GE";
            return code;
        }

        public string GenerateTokenId()
        {
            // Get the current year
            int year = DateTime.Now.Year;

            // Generate a random 4-digit number
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999); // Generate a 4-digit random number

            // Combine the year and random number to form the registration ID
            string regId = "TK" + year.ToString() + randomNumber.ToString();

            return regId;
        }

        public string GenerateRegId()
        {
            // Get the current year
            int year = DateTime.Now.Year;

            // Generate a random 4-digit number
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999); // Generate a 4-digit random number

            // Combine the year and random number to form the registration ID
            string regId = "SP" + year.ToString() + randomNumber.ToString();

            return regId;
        }




        public bool SendQuotationEmail(string email, string subject, string body, Stream attachmentStream, string attachmentFileName)
        {
            var fromEmail = new MailAddress("syrophage@gmail.com", "SYROPHAGE");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "unrjpaiutcscfree"; // This should be replaced with your actual email password

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
                // Attach PDF
                if (attachmentStream != null)
                {
                    message.Attachments.Add(new Attachment(attachmentStream, attachmentFileName));
                }

                try
                {
                    smtp.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    // Handle exception or log error
                    return false;
                }
            }
        }

        public void SendJobAddedEmail(string email, MailVm obj)
        {
            try
            {
                var smtpServer = "smtp.gmail.com";
                var smtpPort = 587;
                var smtpEnableSsl = true;
                var smtpUsername = "syrophage@gmail.com";
                var smtpPassword = "unrjpaiutcscfree";

                using (var smtpClient = new System.Net.Mail.SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.EnableSsl = smtpEnableSsl;
                    smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);

                    var message = new System.Net.Mail.MailMessage();
                    message.From = new System.Net.Mail.MailAddress(smtpUsername);
                    message.To.Add(email);
                    message.Subject = "New Entity Added";

                    // Determine which property is not null and construct the email body accordingly
                    if (obj.Productvm != null)
                    {
                        message.Body = $"A new Product has been added in Syrophage\n\nEntity: {obj.Productvm.productname}\nDescription: {obj.Productvm.Description}";
                    }
                    else if (obj.Servicevm != null)
                    {
                        message.Body = $"A new Service has been added in Syrophage\n\nEntity: {obj.Servicevm.servicename}\nDescription: {obj.Servicevm.Description}";
                    }
                    else if (obj.ProductCatVm != null)
                    {
                        message.Body = $"A new Product Category has been added in Syrophage\n\nEntity: {obj.ProductCatVm.CategoryName}\nDescription: {obj.ProductCatVm.CategoryDescription}";
                    }
                    else if (obj.SerCatVm != null)
                    {
                        message.Body = $"A new Service category has been added in Syrophage\n\nEntity: {obj.SerCatVm.ServiceCategoryName}\nDescription: {obj.SerCatVm.ServiceCategoryDescription}";
                    }
                    else
                    {
                        // Handle the case when all properties are null
                        message.Body = "A new job has been added in JOBPORTAL";
                    }

                    smtpClient.Send(message);
                }
            }
            catch (Exception ex)
            {
                // Handle email sending errors (e.g., log the error, send a notification, etc.)
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw; // Rethrow the exception or handle it as appropriate
            }
        }



    }
}
