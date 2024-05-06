using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    internal class ServicesRepository : Repository<Service>, IServicesRepository
    {
        public readonly ApplicationDbContext db;
        public ServicesRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public List<Service> GetByCategoryName(string name)
        {
            return db.Services.Where(e => e.Category == name).ToList();
        }

        public Service GetById(int Id)
        {
            return db.Services.Find(Id);
        }

        public void Update(Service obj)
        {
            db.Services.Update(obj);
        }
    }
}