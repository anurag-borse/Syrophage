using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    public class UserCouponRepository : Repository<UserCoupon>, IUserCouponRepository
    {
        private readonly ApplicationDbContext _db;
        public UserCouponRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
