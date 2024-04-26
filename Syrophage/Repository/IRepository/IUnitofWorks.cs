namespace Syrophage.Repository.IRepository
{
    public interface IUnitofWorks
    {
        IContactRepository Contact { get; }
        public void Save();

    }
}
