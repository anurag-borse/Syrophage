using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly ApplicationDbContext _db;

        public AdminRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Admin GetById(int Id)
        {
            return _db.Admins.Find(Id);
            
        }

        public void Update(Admin obj)
        {
            _db.Admins.Update(obj);
        }
    }
    
}
