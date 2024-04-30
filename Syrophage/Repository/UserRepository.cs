using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    internal class UserRepository : Repository<Users>, IUserRepository
    {

        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {


            _db = db;
        }

        public Users GetById(int Id)
        {
            return _db.Users.Find(Id);
        }

        public Users GetByname(string Name)
        {
            return _db.Users.FirstOrDefault(u => u.Name == Name);
        }

        public void Update(Users obj)
        {
            _db.Users.Update(obj);
        }
    }
}