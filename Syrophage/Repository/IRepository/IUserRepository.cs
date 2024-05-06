using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IUserRepository : IRepository<Users>
    {
        int Count();
        Users GetById(int Id);

        Users GetByname(string Name);


        void Update(Users obj);

        
        
    }
}
