using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    internal class QuatationFixRepository : Repository<Quatation_details_fix>, IQuatationFixRepository
    {
        private readonly ApplicationDbContext _db;
        public QuatationFixRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Quatation_details_fix GetById(int Id)
        {
            return _db.Quatations_fix.Find(Id);
        }

        public void Update(Quatation_details_fix obj)
        {
            _db.Quatations_fix.Update(obj);
        }
    }
}