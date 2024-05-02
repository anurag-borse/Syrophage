using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    public class CategoriesRepository : Repository<Categories>, ICategoriesRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoriesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Categories GetById(int Id)
        {
            return _db.Categories.Find(Id);
        }

        public void Update(Categories obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
