using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IServicesCategoriesRepository: IRepository<ServiceCategory>
    {
        ServiceCategory GetById(int Id);

        void Update(ServiceCategory obj);
    }
}