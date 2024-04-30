using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        Coupon GetById(int id);
        void Update(Coupon coupon);
    }
}
