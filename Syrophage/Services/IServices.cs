using Syrophage.Models;
using Syrophage.Models.ViewModel;

namespace Syrophage.Services
{
    public interface IServices
    {

        public bool SendThanksEmail(string email);

        public bool SendRegistrationEmail(string email, string regId);


        public bool SendActivationEmail(string email, string password);
        public bool SendQuotationEmail(string email, string subject, string body, Stream attachmentStream, string attachmentFileName);
         public void SendJobAddedEmail(string email, MailVm obj);
        public string GenerateCouponCode();

        public string GenerateTokenId();
        public string GenerateRegId();


    }
}
