using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Admin GetById(int Id);
        void Update(Admin obj);

    }
}
