namespace Syrophage.Repository.IRepository
{
    public interface IUnitofWorks
    {
        IContactRepository Contact { get; }
      

        INewsletterRepository Newsletter { get; }


        IUserRepository User { get; }

        ITokenRepository Token { get; }

        ICouponRepository Coupon { get; }

        IUserCouponRepository UserCoupon { get; }


        public void Save();
    }
}
