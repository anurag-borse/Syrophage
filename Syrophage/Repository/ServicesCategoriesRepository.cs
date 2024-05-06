using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    internal class ServicesCategoriesRepository : Repository<ServiceCategory>, IServicesCategoriesRepository
    {
        public readonly ApplicationDbContext db;
        public ServicesCategoriesRepository(ApplicationDbContext db) : base(db)
        {

            this.db = db;
        }

        public ServiceCategory GetById(int Id)
        {
            return db.ServiceCategories.Find(Id);
        }

        public void Update(ServiceCategory obj)
        {
            db.ServiceCategories.Update(obj);
        }
    }
}