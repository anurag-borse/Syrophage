using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    internal class QuatationServiceRepository : Repository<Qua_Service>, IQuatationServiceRepository
    {
        private readonly ApplicationDbContext db;
        public QuatationServiceRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public Qua_Service GetById(int Id)
        {
            return db.Quatations_Services.Find(Id);
        }

        public void Update(Qua_Service obj)
        {
            db.Quatations_Services.Update(obj);
        }
    }
}