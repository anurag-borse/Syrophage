using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    internal class NewsletterRepository : Repository<Newsletter>, INewsletterRepository
    {

        private readonly ApplicationDbContext _db;
        public NewsletterRepository(ApplicationDbContext db) : base(db)
        {

            _db = db;
        }

        public Newsletter GetById(int Id)
        {
            return _db.Newsletters.Find(Id);
        }

        public List<Newsletter> Search(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public void Update(Newsletter obj)
        {
            _db.Newsletters.Update(obj);
        }
    }
}