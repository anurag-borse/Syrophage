using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        public CouponRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int Count()
        {
            return _db.Coupons.Count(); 
        }

        public Coupon GetById(int id)
        {
            return _db.Coupons.Find(id);
        }

        public void Update(Coupon coupon)
        {
            _db.Coupons.Update(coupon);
        }
    }
}
