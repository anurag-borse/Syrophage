namespace Syrophage.Repository.IRepository
{
    public interface IUnitofWorks
    {
        IContactRepository Contact { get; }
      

        INewsletterRepository Newsletter { get; }


        IUserRepository User { get; }

        ITokenRepository Token { get; }

        public void Save();
    }
}
