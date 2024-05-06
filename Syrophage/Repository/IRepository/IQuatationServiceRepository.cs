using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IQuatationServiceRepository: IRepository<Qua_Service>
    {

        Qua_Service GetById(int Id);

        void Update(Qua_Service obj);
    }
}