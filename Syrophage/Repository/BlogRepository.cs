using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    internal class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly ApplicationDbContext db;
        public BlogRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public Blog GetById(int Id)
        {
            return db.Blogs.Find(Id);
        }

        public void Update(Blog obj)
        {
            db.Blogs.Update(obj);
        }
    }
}