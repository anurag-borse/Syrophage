using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IUserRepository : IRepository<Users>
    {

        Users GetById(int Id);

        Users GetByname(string Name);


        void Update(Users obj);

        
    }
}
