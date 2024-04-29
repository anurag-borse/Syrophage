namespace Syrophage.Services
{
    public interface IServices
    {

        public bool SendThanksEmail(string email);

        public bool SendRegistrationEmail(string email, string regId);


        public bool SendActivationEmail(string email, string password);
    }
}
