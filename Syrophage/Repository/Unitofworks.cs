using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    public class Unitofworks : IUnitofWorks
    {
        private readonly ApplicationDbContext _db;
        public IContactRepository Contact { get; set; }

        public INewsletterRepository Newsletter { get; set; }

        public IUserRepository User { get; set; }

        public ITokenRepository Token { get; set; }

        public IOrderRepository Orders { get; set; }

        public ICouponRepository Coupon { get; set; }

        public IUserCouponRepository UserCoupon { get; set; }

        public ICategoriesRepository Categories { get; set; }

        public IProductsRepository Product { get; set; }

        public IServicesCategoriesRepository ServiceCategories { get; set; }

        public IServicesRepository Services { get; set; }

        public IAdminRepository Admin { get; set;}

        public IQuatationFixRepository QuatationFix { get; set; }

        public IQuatationServiceRepository QuaService { get; set; }

        public IQuatationFormRepository QuaForm { get; set; }

        public Unitofworks(ApplicationDbContext _db)
        {
            this._db = _db;
            Contact = new ContactRepository(_db);
            Newsletter = new NewsletterRepository(_db);
            User = new UserRepository(_db);
            Token = new TokenRepository(_db);
            Orders = new OrderRepository(_db);
            Coupon = new CouponRepository(_db);
            Product = new ProductsRepository(_db);
            UserCoupon = new UserCouponRepository(_db);
            Categories = new CategoriesRepository(_db);
            ServiceCategories = new ServicesCategoriesRepository(_db);
            Services = new ServicesRepository(_db);
            Admin = new AdminRepository(_db);
            QuatationFix = new QuatationFixRepository(_db);
            QuaService = new QuatationServiceRepository(_db);
            QuaForm = new QuatationFormRepository(_db);

        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }

}
