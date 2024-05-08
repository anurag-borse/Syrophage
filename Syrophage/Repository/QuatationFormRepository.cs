using Microsoft.EntityFrameworkCore;
using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    internal class QuatationFormRepository : Repository<QuotationFormData>, IQuatationFormRepository
    {
        private readonly ApplicationDbContext db;
        public QuatationFormRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public IQueryable<QuotationFormData> GetAlll()
        {
            return db.Quatations_Data.Include(q => q.Services);
        }

        public QuotationFormData GetById(int Id)
        {
            return db.Quatations_Data.Find(Id);
        }

        public void Update(QuotationFormData obj)
        {
            db.Quatations_Data.Update(obj);
        }
    }
}