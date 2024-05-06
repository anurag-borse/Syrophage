using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    internal class ContactRepository : Repository<Contact>, IContactRepository
    {
        private readonly ApplicationDbContext _db;
        public ContactRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int Count()
        {
            return _db.contacttb.Count();   
        }

        public Contact GetById(int Id)
        {
            return _db.contacttb.Find(Id);
        }

        public List<Contact> Search(string searchTerm)
        {
            var search = _db.contacttb.Where(x => x.name.Contains(searchTerm) || x.Email.Contains(searchTerm) || x.phone.Contains(searchTerm) || x.message.Contains(searchTerm));
            return search.ToList();
        }


        public void Update(Contact obj)
        {
            _db.contacttb.Update(obj);
        }
    }
}