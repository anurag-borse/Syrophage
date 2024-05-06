using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IQuatationFixRepository : IRepository<Quatation_details_fix>
    {


        Quatation_details_fix GetById(int Id);

        void Update(Quatation_details_fix obj);

    }
}