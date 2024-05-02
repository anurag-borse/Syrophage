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
    }
}
