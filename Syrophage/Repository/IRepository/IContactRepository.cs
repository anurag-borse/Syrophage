using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IContactRepository : IRepository<Contact>
    {
        int Count();
        Contact GetById(int Id);
        List<Contact> Search(string searchTerm);

        void Update(Contact obj);
    }
}