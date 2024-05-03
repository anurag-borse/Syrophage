using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IServicesRepository: IRepository<Service>
    {
        Service GetById(int Id);

        void Update(Service obj);

        List<Service> GetByCategoryName(String name);
    }
}