using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Blog GetById(int Id);

        void Update(Blog obj);
    }
}