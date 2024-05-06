using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        int Count();
        Coupon GetById(int id);
        void Update(Coupon coupon);
    }
}
