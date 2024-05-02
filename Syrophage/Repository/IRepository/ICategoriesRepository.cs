using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface ICategoriesRepository : IRepository<Categories>
    {
        Categories GetById(int Id);

        void Update(Categories obj);
    }
}
