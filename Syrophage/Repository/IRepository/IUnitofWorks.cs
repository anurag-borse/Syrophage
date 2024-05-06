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

        IProductsRepository Product { get; }

        IServicesCategoriesRepository ServiceCategories { get; }

        IServicesRepository Services { get; }

        IAdminRepository Admin { get; }


        public void Save();
    }
}
