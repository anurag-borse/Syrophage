namespace Syrophage.Repository.IRepository
{
    public interface IUnitofWorks
    {
        IContactRepository Contact { get; }
      

        INewsletterRepository Newsletter { get; }


        IUserRepository User { get; }

        ITokenRepository Token { get; }

        ICouponRepository Coupon { get; }
        IOrderRepository Orders { get; }


        IUserCouponRepository UserCoupon { get; }

        ICategoriesRepository Categories { get; }


        public void Save();
    }
}
