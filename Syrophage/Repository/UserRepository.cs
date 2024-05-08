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

        public int Count()
        {
            return _db.Users.Count();
        }

        public Users GetByemail(string Email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == Email);
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