using Microsoft.EntityFrameworkCore;
using Syrophage.Models;

namespace Syrophage.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }

        public DbSet<Contact> contacttb { get; set; }

        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<UserCoupon> UserCoupons { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Quatation_details_fix> Quatations_fix { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, email = "admin@gmail.com", role = "Admin" });

            modelBuilder.Entity<Admin>().HasData(new Admin { Id = 1, Email = "admin@gmail.com", Password = "Admin@123" });

            modelBuilder.Entity<Quatation_details_fix>().HasData(new Quatation_details_fix
            {
                id = 1,
                Cname = "SYROPHAGE IN PRIVATE LIMITED",
                Cemail = "Syrophage@gmail.com",
                CPhoneNo = "74475 08124",
                CAboutUs = "At Syrophage, we're not just another startup; we're a passionate team of individuals driven \r\nby innovation and collaboration. Our name, derived from the initials of our founding \r\nmembers, symbolizes the unity and collective spirit that defines our organization. By \r\ncombining the words 'synergy' and 'age,' we embrace the power of collaboration and the \r\npromise of a new era. Additionally, we believe in giving back; for every service, we contribute \r\na portion to society, ensuring our impact extends beyond business.",
                CMethodology = "Our mission is to empower businesses to thrive by offering a comprehensive suite of \r\nservices, including but not limited to admin supplies, corporate advisories, employee \r\nengagement activities, customized gifts, greetings mail support, corporate events \r\nmanagement, and much more. We aim to be the go-to partner for corporations seeking to \r\nunlock their full potential and foster a harmonious work environment."
            });
        }

    }
}
