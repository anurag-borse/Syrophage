using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface INewsletterRepository : IRepository<Newsletter>
    {
        int Count();
        Newsletter GetById(int Id);
        List<Newsletter> Search(string searchTerm);

        void Update(Newsletter obj);
    }
}